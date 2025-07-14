using UnityEngine;

public class CarEffectsController : MonoBehaviour {
    [Header("Drift Effects")]
    [SerializeField] private ParticleSystem leftDriftParticles;
    [SerializeField] private ParticleSystem rightDriftParticles;
    [SerializeField] private TrailRenderer leftTrailRenderer;
    [SerializeField] private TrailRenderer rightTrailRenderer;

    [Header("Effect Settings")]
    [SerializeField] private bool useParticles = true;
    [SerializeField] private bool useTrails = true;

    private bool isDrifting = false;

    private void Start() {
        SubscribeToEvents();
        InitializeEffects();
    }

    private void OnDestroy() {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents() {
        CarEvents.onDriftStarted += OnDriftStarted;
        CarEvents.onDriftEnded += OnDriftEnded;
        CarEvents.onResetCar += OnCarReset;
    }

    private void UnsubscribeFromEvents() {
        CarEvents.onDriftStarted -= OnDriftStarted;
        CarEvents.onDriftEnded -= OnDriftEnded;
        CarEvents.onResetCar -= OnCarReset;
    }

    private void InitializeEffects() {
        if (useParticles) {
            if (leftDriftParticles != null) leftDriftParticles.Stop();
            if (rightDriftParticles != null) rightDriftParticles.Stop();
        }

        if (useTrails) {
            if (leftTrailRenderer != null) leftTrailRenderer.emitting = false;
            if (rightTrailRenderer != null) rightTrailRenderer.emitting = false;
        }
    }

    private void OnDriftStarted() {
        if (isDrifting) return;

        isDrifting = true;
        ActivateDriftEffects();
    }

    private void OnDriftEnded() {
        if (!isDrifting) return;

        isDrifting = false;
        DeactivateDriftEffects();
    }

    private void OnCarReset() {
        isDrifting = false;
        DeactivateDriftEffects();
        ClearTrails();
    }

    private void ActivateDriftEffects() {
        if (useParticles) {
            if (leftDriftParticles != null && !leftDriftParticles.isPlaying)
                leftDriftParticles.Play();

            if (rightDriftParticles != null && !rightDriftParticles.isPlaying)
                rightDriftParticles.Play();
        }

        if (useTrails) {
            if (leftTrailRenderer != null)
                leftTrailRenderer.emitting = true;

            if (rightTrailRenderer != null)
                rightTrailRenderer.emitting = true;
        }
    }

    private void DeactivateDriftEffects() {
        if (useParticles) {
            if (leftDriftParticles != null && leftDriftParticles.isPlaying)
                leftDriftParticles.Stop();

            if (rightDriftParticles != null && rightDriftParticles.isPlaying)
                rightDriftParticles.Stop();
        }

        if (useTrails) {
            if (leftTrailRenderer != null)
                leftTrailRenderer.emitting = false;

            if (rightTrailRenderer != null)
                rightTrailRenderer.emitting = false;
        }
    }

    private void ClearTrails() {
        if (leftTrailRenderer != null)
            leftTrailRenderer.Clear();

        if (rightTrailRenderer != null)
            rightTrailRenderer.Clear();
    }
}