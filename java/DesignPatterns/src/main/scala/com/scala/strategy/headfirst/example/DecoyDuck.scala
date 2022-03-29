package com.scala.strategy.headfirst.example

import Strategies._


case class DecoyDuck(flyingStrategy:FlyingStrategy = Strategies.flyNoWayStrategy,
                      quackStrategy:QuackStrategy = Strategies.muteStrategy) extends Duck {

  def display: Unit = System.out.println("I'm a decoy")

}
