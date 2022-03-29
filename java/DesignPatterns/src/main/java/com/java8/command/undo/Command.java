package com.java8.command.undo;

public interface Command {
	public void execute();
	public void undo();
}
