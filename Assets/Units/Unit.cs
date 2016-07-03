﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    [Header("Unit Stats:")]
    public int hp;
    public int mp;
    public float walkSpeed;
    public float runSpeed;
    public float attackSpeed;
    public float attackRange;
    public float damage;
    public float defense;
    [Range(0.1f, 3)]
    public float radius;
    [Range(0, 100)]
    public float sight;
    [Space(5)]

    [Header("Skill Object References:")]
    public ParticleSystemPlayer Skill0;
    public ParticleSystemPlayer Skill1;
    public ParticleSystemPlayer Skill2;
    public ParticleSystemPlayer Skill3;
    [Space(5)]

    [Header("Team:")]
    public Team team = Team.T1;

    protected NavMeshAgent agent;
    protected Animator animator;
    protected bool isHold = false;
    protected bool isRunning = false;
    protected bool isAttacking = false;

    public Command command = new Command(CommandType.None);

    private AI ai;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ai = GetComponent<AI>();

        command.type = CommandType.None;
        agent.radius = radius;
    }

    protected virtual void FixedUpdate()
    {
        agent.speed = (isRunning) ? runSpeed : walkSpeed;

        SetAnimatorSpeedIfAgentIsMovingProperly();

        animator.SetBool("Attack", isAttacking);
    }

    private void SetAnimatorSpeedIfAgentIsMovingProperly()
    {
        if (animator != null)
        {
            if (!command.type.Equals(CommandType.None) && !command.type.Equals(CommandType.Hold) && !command.type.Equals(CommandType.Busy))
            {
                animator.SetFloat("Speed", agent.velocity.magnitude);
            }
            else
            {
                animator.SetFloat("Speed", 0);
            }
        }
    }

    public Command GetCommand() { return command; }
    public void SetCommand(Command command) { this.command = command; }
    public void SetAttacking(bool isAttacking) { this.isAttacking = isAttacking; }
    public bool IsHold() { return isHold; }

    public virtual bool IsWaypointNecessary(Command command)
    {
        bool ret = false;
        switch (command.type)
        {
            case CommandType.Move:
            case CommandType.Attack:
                ret = true;
                break;
        }
        return ret;
    }

    public virtual void PerformCommand(Command command)
    {
        if (command.type.Equals(CommandType.Busy)) Debug.LogWarning("Perform Command BUSY ????");
        agent.stoppingDistance = 0;

        switch (command.type)
        {
            case CommandType.Hold:
                this.command = command;
                isHold = true;
                agent.ResetPath();
                break;
            case CommandType.Stop:
                this.command = new Command(CommandType.None);
                isHold = false;
                agent.ResetPath();
                break;
            case CommandType.Move:
                this.command = command;
                isHold = false;
                agent.SetDestination(command.pos);
                if (ai != null) ai.setAiTimeUntilCheck(5);
                break;
            case CommandType.Attack:
                this.command = command;
                isHold = false;
                agent.SetDestination(command.pos);
                if (ai != null) ai.setAiTimeUntilCheck(5);
                agent.stoppingDistance = (command.strictAttack) ? attackRange + radius + command.unitToAttack.radius : 0;
                break;
        }
    }
}

public enum Team
{
    T1, T2, T3, T4, T5, T6, T7, T8
}
