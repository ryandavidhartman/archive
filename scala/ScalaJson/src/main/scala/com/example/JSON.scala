package com.example

import scala.collection.immutable.ListMap

trait JSON

case class JSeq(elements: Seq[JSON]) extends JSON
case class JObj(bindings: ListMap[String, JSON]) extends JSON
case class JNum(number: Double) extends JSON
case class JString(string: String) extends JSON
case class JBool(boolean: Boolean) extends JSON
case object JNull extends JSON


object JSON {

  type JBinding = (String, JSON)

  def toString(json: JSON): String = json match {
    case JSeq(elems) => s"[${elems map toString mkString(", ")}]"
    case JObj(bindings) => {
      val  assocs = bindings map {
        case (k,v) => s""""${k}": ${toString(v)}"""
      }
      s"{${assocs  mkString(", ")}}"
    }
    case JNum(n) => if(n == n.asInstanceOf[Int]) n.asInstanceOf[Int].toString else n.toString
    case JString(s) => s""""${s}""""
    case JBool(b) => b.toString
    case JNull => "null"
  }

}

