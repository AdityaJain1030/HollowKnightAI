using System.Collections;
using UnityEngine;

namespace HollowKnightAI.Utils
{
	public enum DataTypes
	{
		Byte,
		Short,
		Int,
		Long,
		Float,
		Double,
		Bool,
		String,
		ByteArray,
		ShortArray,
		IntArray,
		LongArray,
		FloatArray,
		DoubleArray,
		BoolArray,
		StringArray
	}
	public abstract class Serializable<T>
	{
		public abstract DataTypes Type { get; }
		public abstract int Size { get; }
		public abstract T data { get; set; }
		public abstract byte[] Serialize();
		public abstract T Deserialize(byte[] buffer, ref int index);

	}
}