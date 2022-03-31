package com.example

import org.scalatest.FlatSpec

class JSONSpecs extends FlatSpec {

  import TestData._

  "The JSON converterter" should "do basic conversions" in {
    assert(testPersonJsonString.replaceAll("\\s", "") === JSON.toString(testPersonJsonObj).replaceAll("\\s", ""))
}


}

