using System;
using UnityEngine;

// Mainly taken from the following:
// https://github.com/Math-Man/WebDemos/blob/master/Assets/Demos/SphericalMotion/Scripts/SphericalCoord.cs
// Did some minor refactors for having getters & setters, and removed methods that I won't be using.
namespace Wright.Library.Math
{
    [Serializable]
    public class SphericalCoordinates
    {
        public const float TwoPI = Mathf.PI * 2;

        [SerializeField] private float _radialDistance;
        public float RadialDistance
        {
            get => _radialDistance;
            set => _radialDistance = value;
        }
        
        [SerializeField] private float _polar;
        public float Polar
        {
            get => _polar;
            set => _polar = SphericalClamp(value);
        }
        
        [SerializeField] private float _elevation;
        public float Elevation
        {
            get => _elevation;
            set => _elevation = SphericalClamp(_elevation);
        }
        
        /// <summary>
        /// Spherically clamps the value between 0 and TwoPI
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float SphericalClamp(float value)
        {
            var flatValue = value >= 0 ? value : TwoPI - Mathf.Abs(value % TwoPI);
            return flatValue % TwoPI;
        }

        /// <summary>
        ///  <para> Creates a new spherical position with given Radial Distance, Elevation, Polar coordinates</para>
        /// </summary>
        /// <param name="r"></param>
        /// <param name="q"></param>
        /// <param name="p"></param>
        public SphericalCoordinates(float r, float q, float p)
        {
            this._radialDistance = r;
            this._polar = SphericalClamp(q);
            this._elevation = SphericalClamp(p);
        }

        /// <summary>
        /// Converts cartesian coordinates to spherical coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static SphericalCoordinates FromCartesian(float x, float y, float z)
        {
            return FromCartesian(new Vector3(x, y, z));
        }

        /// <summary>
        /// Converts cartesian coordinates to spherical coordinates
        /// </summary>
        /// <param name="cartesianCoord"></param>
        /// <returns></returns>
        public static SphericalCoordinates FromCartesian(Vector3 cartesianCoord)
        {
            if (cartesianCoord.x.CompareTo(0) == 0)
                cartesianCoord.x = Mathf.Epsilon;
            var radDist = cartesianCoord.magnitude; // Here, we're assuming that we are centered on the origin.
            var pol = Mathf.Atan(cartesianCoord.z / cartesianCoord.x);
            if (cartesianCoord.x < 0)
                pol += Mathf.PI;
            if (radDist.CompareTo(0) == 0)
                radDist = Mathf.Epsilon;
            var elev = Mathf.Asin(cartesianCoord.y / radDist);
            return new SphericalCoordinates(radDist, pol, elev);
        }
        
        /// <summary>
        /// Converts spherical position to cartesian position
        /// </summary>
        /// <returns></returns>
        public Vector3 ToCartesian()
        {
            float a = _radialDistance * Mathf.Cos(_elevation);
            return new Vector3(
                a * Mathf.Cos(_polar),
                _radialDistance * Mathf.Sin(_elevation),
                a * Mathf.Sin(_polar));
        }

        /// <summary>
        /// Converts spherical position to cartesian position
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public static Vector3 ToCartesian(SphericalCoordinates coords)
        {
            return coords.ToCartesian();
        }
    }
}