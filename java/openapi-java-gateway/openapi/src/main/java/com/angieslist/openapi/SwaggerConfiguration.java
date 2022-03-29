package com.angieslist.openapi;

import com.angieslist.framework.service.ServiceRequest;
import org.springframework.boot.autoconfigure.condition.ConditionalOnExpression;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import springfox.documentation.builders.ApiInfoBuilder;
import springfox.documentation.builders.PathSelectors;
import springfox.documentation.builders.RequestHandlerSelectors;
import springfox.documentation.service.ApiInfo;
import springfox.documentation.spi.DocumentationType;
import springfox.documentation.spring.web.plugins.Docket;
import springfox.documentation.swagger2.annotations.EnableSwagger2;

import static com.google.common.collect.Sets.newHashSet;


@Configuration
@EnableSwagger2
public class SwaggerConfiguration {

  @Bean
  @ConditionalOnExpression("!('${app.env}' matches '.*prod.*')")
    public Docket api() {
        return new Docket(DocumentationType.SWAGGER_2)
                .genericModelSubstitutes(ServiceRequest.class)
                .select()
                .apis(RequestHandlerSelectors.basePackage("com.angieslist.openapi"))
                .build()
                .forCodeGeneration(true)
                .produces(newHashSet("application/json"))
                .apiInfo(metadata())
                .useDefaultResponseMessages(false);
    }

    @Bean
    @ConditionalOnExpression("'${app.env}' matches '.*prod.*'")
    public Docket blankApi() {
        return new Docket(DocumentationType.SWAGGER_2)
                .genericModelSubstitutes(ServiceRequest.class)
                .select()
                .paths(PathSelectors.none()) // Hides all paths in production
                .build()
                .forCodeGeneration(true)
                .produces(newHashSet("application/json"))
                .apiInfo(metadata())
                .useDefaultResponseMessages(false);
    }

    private ApiInfo metadata() {
        return new ApiInfoBuilder()
                .title("openapi")
                .description("API for interacting with openapi")
                .version("1.0")
                .build();
    }
}
