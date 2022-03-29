package com.angieslist.openapi;

import com.angieslist.openapi.proxy.version1.ExampleRequest;
import com.angieslist.openapi.proxy.version1.ExampleResponse;
import com.angieslist.framework.cache.CacheKeyGenerator;
import com.angieslist.framework.cache.LoggingCacheErrorHandler;
import com.angieslist.framework.cache.RedisOperationsFactory;
import com.angieslist.framework.http.RestOperationsFactory;
import com.angieslist.framework.service.HttpServiceProxy;
import com.angieslist.framework.service.ServiceProxy;
import com.angieslist.framework.util.ConvertingRetryOperations;
import com.angieslist.framework.util.ConvertingRetryTemplate;
import com.angieslist.framework.util.RequestAwareHystrixConcurrencyStrategy;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.netflix.hystrix.strategy.HystrixPlugins;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.cache.CacheManager;
import org.springframework.cache.annotation.CachingConfigurerSupport;
import org.springframework.cache.annotation.EnableCaching;
import org.springframework.cache.interceptor.CacheErrorHandler;
import org.springframework.cache.interceptor.KeyGenerator;
import org.springframework.cloud.client.circuitbreaker.EnableCircuitBreaker;
import org.springframework.cloud.netflix.hystrix.dashboard.EnableHystrixDashboard;
import org.springframework.context.ApplicationListener;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.context.event.ContextRefreshedEvent;
import org.springframework.data.redis.cache.RedisCacheManager;
import org.springframework.data.redis.connection.RedisConnectionFactory;
import org.springframework.retry.backoff.FixedBackOffPolicy;
import org.springframework.retry.policy.SimpleRetryPolicy;
import org.springframework.retry.support.RetryTemplate;
import org.springframework.scheduling.annotation.EnableAsync;
import org.springframework.web.client.RestOperations;

@org.springframework.context.annotation.Configuration
@EnableAsync
@EnableCaching
@EnableCircuitBreaker
@EnableHystrixDashboard
@ComponentScan("com.angieslist")
public class Configuration extends CachingConfigurerSupport implements ApplicationListener<ContextRefreshedEvent> {
    @Autowired
    private RedisOperationsFactory redisOperationsFactory;

    @Autowired
    private RestOperationsFactory restOperationsFactory;

    @Override
    public void onApplicationEvent(ContextRefreshedEvent event) {
        HystrixPlugins.getInstance().registerConcurrencyStrategy(new RequestAwareHystrixConcurrencyStrategy());
    }

    @Bean
    public RestOperations restOperations(ObjectMapper objectMapper) {
        return restOperationsFactory.build(objectMapper);
    }

    @Bean
    CacheManager exampleCacheManager(RedisConnectionFactory redisConnectionFactory) {
        return new RedisCacheManager(redisOperationsFactory.build(redisConnectionFactory, ExampleResponse.class));
    }

    @Bean
    CacheErrorHandler cacheErrorHandler() {
        return new LoggingCacheErrorHandler();
    }

    @Bean
    public KeyGenerator keyGenerator() {
        return new CacheKeyGenerator();
    }


    @Bean
    public ConvertingRetryOperations convertingRetryTemplate() {
        SimpleRetryPolicy retryPolicy = new SimpleRetryPolicy();
        retryPolicy.setMaxAttempts(3);

        FixedBackOffPolicy backOffPolicy = new FixedBackOffPolicy();
        backOffPolicy.setBackOffPeriod(500);

        RetryTemplate retryTemplate = new RetryTemplate();
        retryTemplate.setRetryPolicy(retryPolicy);
        retryTemplate.setBackOffPolicy(backOffPolicy);

        return new ConvertingRetryTemplate(retryTemplate);
    }

    @Bean
    public ServiceProxy exampleProxy(ObjectMapper objectMapper) {
        return new HttpServiceProxy("http://localhost:8080/v1/example", restOperations(objectMapper), objectMapper);
    }

    @Bean
    public ServiceProxy serviceProxy(ObjectMapper objectMapper) {
        // Generic proxy that can be used with any URL, Request, Response
        // Can also subclass ServiceProxy to pin down URL, Request, and Response if calling same service many times
        return new HttpServiceProxy(restOperations(objectMapper), objectMapper);
    }
}
