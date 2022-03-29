package com.scala.strategy.headfirst.example

import Strategies.{FlyingStrategy, QuackStrategy}

case class MallardDuck(flyingStrategy:FlyingStrategy = Strategies.flyWithWingsStrategy,
                       quackStrategy:QuackStrategy = Strategies.quackStrategy) extends Duck {

  def display: Unit = System.out.println("I'm a real Mallard duck")

}
