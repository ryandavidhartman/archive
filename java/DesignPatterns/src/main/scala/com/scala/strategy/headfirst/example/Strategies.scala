package com.scala.strategy.headfirst.example

object Strategies {

  type FlyingStrategy = () => Unit
  type QuackStrategy = () => Unit

  val flyNoWayStrategy: FlyingStrategy = () => System.out.println("I can't fly")
  val flyRocketPoweredStrategy: FlyingStrategy = () => System.out.println("I'm flying with a rocket")
  val flyWithWingsStrategy: FlyingStrategy = () => System.out.println("I'm flying!!")

  val quackStrategy: QuackStrategy = () => System.out.println("Quack!")
  val squeekStrategy: QuackStrategy = () => System.out.println("Squeek!")
  val muteStrategy: QuackStrategy = () => System.out.println("<< Silence >>")
}