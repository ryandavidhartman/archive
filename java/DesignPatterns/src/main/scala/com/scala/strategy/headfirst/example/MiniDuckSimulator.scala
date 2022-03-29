package com.scala.strategy.headfirst.example

import Strategies._

// This Duck example is based of of the Head First Design Patterns
// Strategy pattern.  But may in fact my more like the State Pattern
// https://en.wikipedia.org/wiki/State_pattern


object MiniDuckSimulator {
  def main(args: Array[String]): Unit = {
    System.out.println("Hi From Scala")
    val mallard: MallardDuck = new MallardDuck
    val rubberDuckie: RubberDuck = new RubberDuck
    val decoy: DecoyDuck = new DecoyDuck

    val model: ModelDuck = new ModelDuck

    mallard.quack()
    rubberDuckie.quack()
    decoy.quack()

    model.fly()
    val model2 = new ModelDuck(Strategies.flyRocketPoweredStrategy)
    model2.fly()

  }

}
