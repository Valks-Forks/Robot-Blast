global using Godot;
global using System;

using AnimationNSMP = Godot.AnimationNodeStateMachinePlayback;

namespace RobotBlast;

public partial class Player : CharacterBody2D 
{
    [Export] private int Acceleration          { get; set; } = 500;
    [Export] private int MaxSpeed              { get; set; } = 80;
    [Export] private int Friction              { get; set; } = 500;

    private AnimationPlayer RobotAnimation     { get; set; }
    private AnimationTree   RobotAnimationTree { get; set; }
	private AnimationNSMP   StateMachine       { get; set; }

    public override void _Ready() 
    {
        RobotAnimation = GetNode<AnimationPlayer>("RobotAnimation");
        RobotAnimationTree = GetNode<AnimationTree>("RobotAnimationTree");
        StateMachine = (AnimationNSMP)RobotAnimationTree.Get("parameters/playback");
    }

    public override void _PhysicsProcess(double d) 
    {
        var delta = (float)d;

        var inputVector = new Vector2
        {
			X = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
			Y = Input.GetActionStrength("move_down" ) - Input.GetActionStrength("move_up"  )
		}.Normalized();

        if (inputVector != Vector2.Zero) 
        {
            RobotAnimationTree.Set("parameters/Idle/blend_position", inputVector);
            RobotAnimationTree.Set("parameters/Run/blend_position", inputVector);
            StateMachine.Travel("Run");

            Velocity = Velocity.MoveToward(inputVector * MaxSpeed, Acceleration * delta);
        } 
        else 
        {
            StateMachine.Travel("Idle");
            Velocity = Velocity.MoveToward(Vector2.Zero, Friction * delta);
        }

        MoveAndSlide();
    }
}
