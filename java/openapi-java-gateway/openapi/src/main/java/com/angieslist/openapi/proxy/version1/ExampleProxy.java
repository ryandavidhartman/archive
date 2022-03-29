package com.angieslist.openapi.proxy.version1;

import com.angieslist.framework.security.SecurityContext;
import com.angieslist.framework.service.ServiceProxy;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.autoconfigure.condition.ConditionalOnProperty;
import org.springframework.stereotype.Component;

import java.util.concurrent.CompletableFuture;

@Component
@ConditionalOnProperty("example.url")
public class ExampleProxy {
    @Autowired
    private ServiceProxy serviceProxy;

    @Value("${example.url}")
    private String url;

    public CompletableFuture<ExampleResponse> request(SecurityContext securityContext, ExampleRequest request) {
        return serviceProxy.request(url, securityContext, request, ExampleResponse.class);
    }
}
