using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("data", "status", "slot")]
	public class ES3UserType_InventoryItem : ES3ObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_InventoryItem() : base(typeof(InventoryItem)){ Instance = this; priority = 1; }


		protected override void WriteObject(object obj, ES3Writer writer)
		{
			var instance = (InventoryItem)obj;
			
			writer.WritePropertyByRef("data", instance.data);
			writer.WriteProperty("status", instance.status, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(ItemStatus)));
			writer.WritePropertyByRef("slot", instance.slot);
		}

		protected override void ReadObject<T>(ES3Reader reader, object obj)
		{
			var instance = (InventoryItem)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "data":
						instance.data = reader.Read<ItemData>();
						break;
					case "status":
						instance.status = reader.Read<ItemStatus>();
						break;
					case "slot":
						instance.slot = reader.Read<InventorySlot>();
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}

		protected override object ReadObject<T>(ES3Reader reader)
		{
			var instance = new InventoryItem();
			ReadObject<T>(reader, instance);
			return instance;
		}
	}


	public class ES3UserType_InventoryItemArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_InventoryItemArray() : base(typeof(InventoryItem[]), ES3UserType_InventoryItem.Instance)
		{
			Instance = this;
		}
	}
}