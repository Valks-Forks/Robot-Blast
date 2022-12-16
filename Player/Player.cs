using Godot;
using System;
using System.Transactions;

public partial class Player : Godot.CharacterBody2D {
    Vector2 Velocity = Vector2.Zero;
    int Acceleration = 500;
    int MaxSpeed = 80;
    int Friction = 500;

    public override void _PhysicsProcess(double delta) {
        Vector2 InputVector = Vector2.Zero;
        InputVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        InputVector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
        InputVector = InputVector.Normalized();

        GD.Print(InputVector);

        if (InputVector != Vector2.Zero) {
            this.Velocity = Velocity.MoveToward(InputVector * MaxSpeed, Acceleration * (float)delta);
        } else {
            this.Velocity = Velocity.MoveToward(Vector2.Zero, Friction * (float)delta);
        }

        MoveAndCollide(Velocity * (float)delta);
    }
}
