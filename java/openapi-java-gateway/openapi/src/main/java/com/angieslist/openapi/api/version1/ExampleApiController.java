package com.angieslist.openapi.api.version1;

import com.angieslist.framework.security.SecurityContext;
import com.angieslist.openapi.proxy.version1.ExampleRequest;
import com.angieslist.openapi.proxy.version1.ExampleResponse;
import com.angieslist.framework.security.RequesterType;
import com.angieslist.framework.security.SecurityService;
import com.angieslist.framework.service.Context;
import com.angieslist.framework.service.ServiceProxy;
import io.swagger.annotations.Api;
import io.swagger.annotations.ApiOperation;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

import javax.servlet.http.HttpServletRequest;
import java.util.concurrent.CompletableFuture;

@Slf4j
@RestController
@RequestMapping("/api/v1")
@Api(tags = {"ExampleServicesV1"}, description = "Example Services Version 1")
public class ExampleApiController {
    @Autowired
    SecurityService securityService;

    @Autowired
    @Qualifier("exampleProxy")
    private ServiceProxy exampleProxy;

    @RequestMapping(value = "/example", method = RequestMethod.GET)
    @ApiOperation(value = "Does example things", nickname = "example", notes = "This one is a great Swagger example")
    public ExampleResponse example(HttpServletRequest httpServletRequest) {
        SecurityContext context = securityService.authenticate(httpServletRequest, RequesterType.AngiesListUser);

        if (!context.isAuthenticated()) {
            log.info("Probably shouldn't let them continue...");
        }

        ExampleRequest exampleRequest = new ExampleRequest();
        exampleRequest.setExampleId(1);

        CompletableFuture<ExampleResponse> first = exampleProxy.request(context, exampleRequest, ExampleResponse.class);
        CompletableFuture<ExampleResponse> second = exampleProxy.request(context, exampleRequest, ExampleResponse.class);

        // Run another future after completion
        second.thenAcceptAsync(exampleResponse -> System.out.println("Doing it! " + exampleRequest));

        // Wait for all results
        CompletableFuture.allOf(first, second);
        // Can now compose

        return first.join();
    }
}
