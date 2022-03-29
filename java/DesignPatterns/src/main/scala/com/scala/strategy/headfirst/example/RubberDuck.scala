package com.scala.strategy.headfirst.example

import Strategies._


case class RubberDuck(flyingStrategy:FlyingStrategy = Strategies.flyNoWayStrategy,
                      quackStrategy:QuackStrategy = Strategies.squeekStrategy) extends Duck {

  def display: Unit = System.out.println("I'm a rubber duckie")

}
