package com.scala.strategy


// See http://alvinalexander.com/scala/how-scala-killed-oop-strategy-design-pattern


object SimpleStrategy {
  def main(args: Array[String]): Unit = {

    // These methods represent various strategies (algorithms) we wish to encapsulate
    def add(a: Int, b: Int) = a + b
    def subtract(a: Int, b: Int) = a - b
    def multiply(a: Int, b: Int) = a * b

    // This method can be used to run any of the above strategies
    def execute(strategy:(Int, Int) => Int, x: Int, y: Int) = strategy(x, y)

    println("Add:         " + execute(add, 3, 4))
    println("Subtract:    " + execute(subtract, 3, 4))
    println("Multiply:    " + execute(multiply, 3, 4))

    // Note it works with functions too
    val addFunction = (x: Int, y: Int) => x + y
    println("AddFunction: " + execute(addFunction, 3, 4))



  }
}
