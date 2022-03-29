package com.java.command.headfirst.example.undo;

public interface Command {
	public void execute();
	public void undo();
}
