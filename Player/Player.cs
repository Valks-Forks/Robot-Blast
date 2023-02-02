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
    private Vector2         InputVec           { get; set; }

    public override void _Ready() 
    {
        RobotAnimation     = GetNode<AnimationPlayer>             ("RobotAnimation");
        RobotAnimationTree = GetNode<AnimationTree>               ("RobotAnimationTree");
        StateMachine       = (AnimationNSMP)RobotAnimationTree.Get("parameters/playback");
    }

    public override void _PhysicsProcess(double d) 
    {
        var delta = (float)d;

		InputVec = GetInput();

        Vector2 moveVec;
		float friction; // not sure if this should be called friction or acceleration or something else

        if (MovingAround()) 
        {
            RobotAnimationTree.Set("parameters/Idle/blend_position", InputVec);
            RobotAnimationTree.Set("parameters/Run/blend_position", InputVec);
            StateMachine.Travel("Run");

			moveVec = InputVec * MaxSpeed;
			friction = Acceleration * delta;
        } 
        else
        {
            StateMachine.Travel("Idle");

            moveVec = Vector2.Zero;
            friction = Friction * delta;
        }

		Velocity = Velocity.MoveToward(moveVec, friction);

		MoveAndSlide();
    }

    private Vector2 GetInput() => new Vector2
	{
		X = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
		Y = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up")
	}.Normalized();

	private bool MovingAround() => InputVec != Vector2.Zero;
}
