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

    public override void _PhysicsProcess(double delta) 
    {
        var InputVector = Vector2.Zero;
        InputVector.X = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
        InputVector.Y = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");
        InputVector = InputVector.Normalized();

        if (InputVector != Vector2.Zero) {
            RobotAnimationTree.Set("parameters/Idle/blend_position", InputVector);
            RobotAnimationTree.Set("parameters/Run/blend_position", InputVector);
            StateMachine.Travel("Run");

            Velocity = Velocity.MoveToward(InputVector * MaxSpeed, Acceleration * (float)delta);
        } 
        
        else {
            StateMachine.Travel("Idle");
            Velocity = Velocity.MoveToward(Vector2.Zero, Friction * (float)delta);
        }

        MoveAndSlide();
    }
}
