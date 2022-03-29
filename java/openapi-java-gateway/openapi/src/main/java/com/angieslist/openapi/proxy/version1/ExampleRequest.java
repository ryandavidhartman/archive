
package com.angieslist.openapi.proxy.version1;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonPropertyOrder;
import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;

import javax.validation.constraints.NotNull;
import io.swagger.annotations.ApiModel;


/**
 * ExampleRequest
 * <p>
 * Pretty generic example request, but cacheable
 *
 */
@JsonInclude(JsonInclude.Include.NON_NULL)
@JsonPropertyOrder({
    "exampleId"
})
@ApiModel("ExampleRequestV1")
public class ExampleRequest {

    /**
     *
     * (Required)
     *
     */
    @JsonProperty("exampleId")
    @NotNull
    private long exampleId;

    /**
     *
     * (Required)
     *
     * @return
     *     The exampleId
     */
    @JsonProperty("exampleId")
    public long getExampleId() {
        return exampleId;
    }

    /**
     *
     * (Required)
     *
     * @param exampleId
     *     The exampleId
     */
    @JsonProperty("exampleId")
    public void setExampleId(long exampleId) {
        this.exampleId = exampleId;
    }

    public ExampleRequest withExampleId(long exampleId) {
        this.exampleId = exampleId;
        return this;
    }

    @Override
    public String toString() {
        return ToStringBuilder.reflectionToString(this);
    }

    @Override
    public int hashCode() {
        return new HashCodeBuilder().append(exampleId).toHashCode();
    }

    @Override
    public boolean equals(Object other) {
        if (other == this) {
            return true;
        }
        if ((other instanceof ExampleRequest) == false) {
            return false;
        }
        ExampleRequest rhs = ((ExampleRequest) other);
        return new EqualsBuilder().append(exampleId, rhs.exampleId).isEquals();
    }

}
