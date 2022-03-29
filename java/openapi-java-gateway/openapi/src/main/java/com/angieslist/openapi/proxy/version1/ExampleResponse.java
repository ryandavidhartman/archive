
package com.angieslist.openapi.proxy.version1;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.annotation.JsonPropertyOrder;
import org.apache.commons.lang.builder.EqualsBuilder;
import org.apache.commons.lang.builder.HashCodeBuilder;
import org.apache.commons.lang.builder.ToStringBuilder;
import io.swagger.annotations.ApiModel;

import java.util.Date;


/**
 * ExampleResponse
 * <p>
 * Pretty generic example response, may be cached
 *
 */
@JsonInclude(JsonInclude.Include.NON_NULL)
@JsonPropertyOrder({
    "date",
    "message"
})
@ApiModel("ExampleResponseV1")
public class ExampleResponse {

    @JsonProperty("date")
    private Date date;
    @JsonProperty("message")
    private String message;

    /**
     *
     * @return
     *     The date
     */
    @JsonProperty("date")
    public Date getDate() {
        return date;
    }

    /**
     *
     * @param date
     *     The date
     */
    @JsonProperty("date")
    public void setDate(Date date) {
        this.date = date;
    }

    public ExampleResponse withDate(Date date) {
        this.date = date;
        return this;
    }

    /**
     *
     * @return
     *     The message
     */
    @JsonProperty("message")
    public String getMessage() {
        return message;
    }

    /**
     *
     * @param message
     *     The message
     */
    @JsonProperty("message")
    public void setMessage(String message) {
        this.message = message;
    }

    public ExampleResponse withMessage(String message) {
        this.message = message;
        return this;
    }

    @Override
    public String toString() {
        return ToStringBuilder.reflectionToString(this);
    }

    @Override
    public int hashCode() {
        return new HashCodeBuilder().append(date).append(message).toHashCode();
    }

    @Override
    public boolean equals(Object other) {
        if (other == this) {
            return true;
        }
        if ((other instanceof ExampleResponse) == false) {
            return false;
        }
        ExampleResponse rhs = ((ExampleResponse) other);
        return new EqualsBuilder().append(date, rhs.date).append(message, rhs.message).isEquals();
    }

}
