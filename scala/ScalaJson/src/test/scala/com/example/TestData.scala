package com.example

import com.example

import scala.collection.immutable.ListMap

case class Address(
  streetAddress: String,
  city: String,
  state: String,
  postalCode: String)

case class PhoneNumber(
  `type`: String,
  number: String)

case class Person(
  firstName: String,
  lastName: String,
  isAlive: Boolean,
  age: Double,
  address: Address,
  phoneNumbers: Seq[PhoneNumber],
  children: Seq[Person],
  spouse: Option[Person])

object TestData {

  val testPersonJsonString =
  """
    |{
    |  "firstName": "John",
    |  "lastName": "Smith",
    |  "isAlive": true,
    |  "age": 27,
    |  "address": {
    |    "streetAddress": "21 2nd Street",
    |    "city": "New York",
    |    "state": "NY",
    |    "postalCode": "10021-3100"
    |  },
    |  "phoneNumbers": [
    |    {
    |      "type": "home",
    |      "number": "212 555-1234"
    |    },
    |    {
    |      "type": "office",
    |      "number": "646 555-4567"
    |    },
    |    {
    |      "type": "mobile",
    |      "number": "123 456-7890"
    |    }
    |  ],
    |  "children": [],
    |  "spouse": null
    |}
  """.stripMargin

  val testPersonJsonObj = JObj(
    ListMap( "firstName" -> JString("John"),
         "lastName" -> JString("Smith"),
         "isAlive" -> JBool(true),
         "age" -> JNum(27),
         "address" -> JObj(ListMap( "streetAddress" -> JString("21 2nd Street"),
           "city" -> JString("New York"),
           "state" -> JString("NY"),
           "postalCode" -> JString("10021-3100")
           )
         ),
         "phoneNumbers" -> JSeq(
          List( JObj(ListMap( "type" -> JString("home"), "number" -> JString("212 555-1234") )),
                JObj(ListMap( "type" -> JString("office"), "number" -> JString("646 555-4567") )),
                JObj(ListMap( "type" -> JString("mobile"), "number" -> JString("123 456-7890") ))
          )
        ),
        "children" -> JSeq(Seq.empty),
        "spouse" -> JNull
      )
  )


  val testPersonClass = Person(
    firstName = "John",
    lastName=  "Smith",
    isAlive =  true,
    age = 27.0,
    address = Address (
      streetAddress = "21 2nd Street",
      city = "New York",
      state = "NY",
      postalCode = "10021-3100"),
    phoneNumbers = Seq(
      PhoneNumber(`type` = "home", number = "212 555-1234"),
      PhoneNumber(`type` = "office", number = "646 555-4567"),
      PhoneNumber(`type` = "mobile", number = "123 456-7890")
    ),
  children = Seq.empty,
  spouse = None)

}
