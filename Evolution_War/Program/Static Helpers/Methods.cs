﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axiom.Math;

namespace Evolution_War
{
	public static class Methods
	{
		public static Int64 GenerateUniqueID { get { return ++CurrentUniqueID; } }
		public static Random Random;
		private static Int64 CurrentUniqueID;

		static Methods()
		{
			Random = new Random();
			CurrentUniqueID = 0;
		}

		public static Double CubicStep(Double pFirstPoint, Double pFirstSlope, Double pSecondPoint, Double pSecondSlope, Double pPercent) // 1st point, 1st speed, 2nd point, 2nd speed, time = [0, 1].
		{
			var t2 = pPercent * pPercent;
			var t3 = t2 * pPercent;

			return ((2 * t3 - 3 * t2 + 1) * pFirstPoint + (t3 - 2 * t2 + pPercent) * pFirstSlope + (-2 * t3 + 3 * t2) * pSecondPoint + (t3 - t2) * pSecondSlope);
		}

		public static Double LinearStep(Double y0, Double y1, Double percent)
		{
			return y0 + (y1 - y0) * percent;
		}

		public static Single LinearStep(Single y0, Single y1, Double percent)
		{
			return (float)(y0 + (y1 - y0) * percent);
		}

		public static Vector2 ToVector2(Vector3 pVector3)
		{
			return new Vector2(pVector3.x, pVector3.y);
		}

		public static Vector2 Projection(Vector2 pBaseVector, Vector2 pProjectedVector)
		{
			return (pProjectedVector.Dot(pBaseVector) / pBaseVector.Dot(pBaseVector)) * pBaseVector;
		}

		public static Vector3 Projection(Vector3 pBaseVector, Vector3 pProjectedVector) // only performs 2D projection
		{
			return (pProjectedVector.Dot(pBaseVector) / pBaseVector.Dot(pBaseVector)) * pBaseVector;
		}

		public static Vector3 AngleToVector(Double pAngleRadians)
		{
			return new Vector3(Math.Cos(pAngleRadians), Math.Sin(pAngleRadians), 0);
		}

		public static Double AngleDifference(Double pFirstAngle, Double pSecondAngle) {
			var diffmod = (pFirstAngle - pSecondAngle + 180) % 360;
			return diffmod + (diffmod < 0 ? 360 : 0) - 180;
		}
	}
}
