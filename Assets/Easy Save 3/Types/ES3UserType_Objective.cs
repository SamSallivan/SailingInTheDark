using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("finished", "objectiveData", "newObjectives", "newObjectivesString", "objectiveDelay", "assignedDialogue", "finishedDialogue", "dialogueDelay", "m_CancellationTokenSource")]
	public class ES3UserType_Objective : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Objective() : base(typeof(Objective)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Objective)obj;
			
			writer.WriteProperty("finished", instance.finished, ES3Type_bool.Instance);
			writer.WritePropertyByRef("objectiveData", instance.objectiveData);
			writer.WriteProperty("newObjectives", instance.newObjectives, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<Objective>)));
			writer.WriteProperty("newObjectivesString", instance.newObjectivesString, ES3Internal.ES3TypeMgr.GetOrCreateES3Type(typeof(System.Collections.Generic.List<System.String>)));
			writer.WriteProperty("objectiveDelay", instance.objectiveDelay, ES3Type_float.Instance);
			writer.WritePropertyByRef("assignedDialogue", instance.assignedDialogue);
			writer.WritePropertyByRef("finishedDialogue", instance.finishedDialogue);
			writer.WriteProperty("dialogueDelay", instance.dialogueDelay, ES3Type_float.Instance);
			writer.WritePrivateField("m_CancellationTokenSource", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Objective)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "finished":
						instance.finished = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "objectiveData":
						instance.objectiveData = reader.Read<ObjectiveData>();
						break;
					case "newObjectives":
						instance.newObjectives = reader.Read<System.Collections.Generic.List<Objective>>();
						break;
					case "newObjectivesString":
						instance.newObjectivesString = reader.Read<System.Collections.Generic.List<System.String>>();
						break;
					case "objectiveDelay":
						instance.objectiveDelay = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "assignedDialogue":
						instance.assignedDialogue = reader.Read<DialogueData>();
						break;
					case "finishedDialogue":
						instance.finishedDialogue = reader.Read<DialogueData>();
						break;
					case "dialogueDelay":
						instance.dialogueDelay = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "m_CancellationTokenSource":
					instance = (Objective)reader.SetPrivateField("m_CancellationTokenSource", reader.Read<System.Threading.CancellationTokenSource>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_ObjectiveArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_ObjectiveArray() : base(typeof(Objective[]), ES3UserType_Objective.Instance)
		{
			Instance = this;
		}
	}
}