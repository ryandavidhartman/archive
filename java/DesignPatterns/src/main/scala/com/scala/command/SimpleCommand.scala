package com.scala.command


// https://pavelfatin.com/design-patterns-in-scala/#command

object Invoker {
  private var history: Seq[() => Unit] = Seq.empty

  def invoke(command: => Unit) { // by-name parameter
    command
    history :+= command _
  }

  def replayHistory: Unit = history map {c => c()}
}


object SimpleCommand {
  def main(args: Array[String]): Unit = {
    Invoker.invoke(println("foo"))

    Invoker.invoke {
      println("bar 1")
      println("bar 2")
    }

    Invoker.replayHistory

  }
}

