using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Models;
using UnityEngine;

public class Consumable : MonoBehaviour, ICollectable
{
    public Viewer Spawner { get; internal set; }

    private Rigidbody rb;
    private Collider collider;
    private bool disabled;

    void FixedUpdate()
    {
        if (disabled && transform.parent) transform.localPosition = Vector3.zero;
    }

    void Update()
    {
        if (disabled && transform.parent) transform.localPosition = Vector3.zero;
    }

    void LateUpdate()
    {
        if (disabled && transform.parent) transform.localPosition = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    public void Disable()
    {
        disabled = true;
        if (transform.parent) transform.localPosition = Vector3.zero;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;
        collider.enabled = false;
    }

    public void Enable()
    {
        disabled = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;
        collider.enabled = true;
    }
}
