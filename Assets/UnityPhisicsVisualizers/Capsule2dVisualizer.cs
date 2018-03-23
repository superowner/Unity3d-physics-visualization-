﻿// Copyright (c) 2018 Archy Piragkov. All Rights Reserved.  Licensed under the MIT license
using UnityEngine;

namespace artics.UnityPhisicsVisualizers
{
    /// <summary>
    /// Draws gizmos of <seealso cref="CapsuleCollider2D"/> which attached for current GameObject.
    /// You can enable and disable visualization by <see cref = "IsVisible" /> parameter
    /// If collider don't change his Offset, Size, and Direction - you can disable <see cref="DynamicBounds"/> to increase performance
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Capsule2dVisualizer : MonoBehaviour
    {
        /// <summary>
        /// Enables or disables rendering of collider
        /// </summary>
        public bool IsVisible = true;

        /// <summary>
        /// Updates bounds of collider every time <see cref="OnDrawGizmos"/> calls. Useful when you changing Offset, Size or Direction of the collider. If you don't just disable to increase performance.
        /// </summary>
        public bool DynamicBounds = true;

        protected CapsuleCollider2D Collider;
        protected Vector2 StartPosition;
        protected Vector2 EndPosition;
        protected Vector2 MultipliedStartPosition;
        protected Vector2 MultipliedEndPosition;
        protected float Radius = 0;

        private void Awake()
        {
            Init();
        }

        [ContextMenu("Tnit")]
        public void Init()
        {
            Collider = GetComponent<CapsuleCollider2D>();
            UpdateBounds();
        }

        void OnDrawGizmos()
        {
            if (!IsVisible)
                return;

            if (DynamicBounds)
                UpdateBounds();

            MultiplyMatrix();

            DrawCapsule(MultipliedStartPosition, MultipliedEndPosition, Radius);
        }

        /// <summary>
        /// Update bounds of collider manually.  Use it if you changed Offset, Size or Direction of the collider.
        /// </summary>
        public void UpdateBounds()
        {
            if (Collider.direction == CapsuleDirection2D.Vertical)
                UpdateBoundsVertical(Collider.size.y, Collider.size.x);
            else
                UpdateBoundsHorizontal(Collider.size.x, Collider.size.y);
        }

        protected void MultiplyMatrix()
        {
            MultipliedStartPosition = transform.localToWorldMatrix.MultiplyPoint(StartPosition);
            MultipliedEndPosition = transform.localToWorldMatrix.MultiplyPoint(EndPosition);
        }

        protected void UpdateBoundsVertical(float height, float radius)
        {
            StartPosition.x = 0;
            EndPosition.x = 0;

            StartPosition.y = height * 0.5f;
            EndPosition.y = StartPosition.y * -1;

            StartPosition += Collider.offset;
            EndPosition += Collider.offset;

            Radius = Mathf.Abs(radius * 0.5f * transform.localScale.x);
        }

        protected void UpdateBoundsHorizontal(float height, float radius)
        {
            StartPosition.y = 0;
            EndPosition.y = 0;

            StartPosition.x = height * 0.5f;
            EndPosition.x = StartPosition.x * -1;

            StartPosition += Collider.offset;
            EndPosition += Collider.offset;

            Radius = Mathf.Abs(radius * 0.5f * transform.localScale.y);
        }

        /// <summary>
        /// You can override drawing method for yout needs
        /// </summary>
        /// <param name="StartPosition"></param>
        /// <param name="EndPosition"></param>
        /// <param name="Radius"></param>
        protected void DrawCapsule(Vector2 StartPosition, Vector2 EndPosition, float Radius)
        {
            DebugExtension.DrawCapsule(StartPosition, EndPosition, Radius);
        }
    }
}