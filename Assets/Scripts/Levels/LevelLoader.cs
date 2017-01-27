// SCIENCE BIRDS: A clone version of the Angry Birds game used for 
// research purposes
// 
// Copyright (C) 2016 - Lucas N. Ferreira - lucasnfe@gmail.com
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>
//

﻿using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;

public class LevelLoader {
	
	public static string ReadXmlLevel(string path) {
	
		string xmlText = "";

		if (path.Contains ("StreamingAssets")) {

			xmlText = File.ReadAllText (path);
		} 
		else {

			string[] stringSeparators = new string[] {"Levels/"};
			string[] arrayPath = path.Split (stringSeparators, StringSplitOptions.None);
			string finalPath = arrayPath [1].Split ('.') [0];

			TextAsset levelData = Resources.Load<TextAsset>("Levels/" + finalPath);
			xmlText = levelData.text;
		}

		return xmlText;
	}
	
	public static ABLevel LoadXmlLevel(string xmlString) {

		ABLevel level = new ABLevel();

		level.pigs = new List<OBjData>();
		level.blocks = new List<OBjData>();
		level.platforms = new List<PlatData>();

		using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
		{
			reader.ReadToFollowing("Level");

			level.width = 1;
			if (reader.GetAttribute ("width") != null) {

				reader.MoveToAttribute("width");
				level.width = (int)Convert.ToInt32 (reader.Value);
			}

			reader.ReadToFollowing("Camera");

			reader.MoveToAttribute("x");
			level.camera.x = (float)Convert.ToDouble (reader.Value);

			reader.MoveToAttribute("y");
			level.camera.y = (float)Convert.ToDouble (reader.Value);

			reader.MoveToAttribute("minWidth");
			level.camera.minWidth = (float)Convert.ToDouble (reader.Value);

			reader.MoveToAttribute("maxWidth");
			level.camera.maxWidth = (float)Convert.ToDouble (reader.Value);
				
			reader.ReadToFollowing("BirdsAmount");
			level.birdsAmount = Convert.ToInt32(reader.ReadElementContentAsString());

			reader.ReadToFollowing("Slingshot");

			reader.MoveToAttribute("x");
			level.slingshot.x = (float)Convert.ToDouble (reader.Value);

			reader.MoveToAttribute("y");
			level.slingshot.y = (float)Convert.ToDouble (reader.Value);

			reader.ReadToFollowing("GameObjects");
			reader.Read ();

			while (reader.Read())
			{
				string nodeName = reader.LocalName;
				if (nodeName == "GameObjects")
					break;

				PlatData abObj = new PlatData();

				reader.MoveToAttribute("type");
				abObj.type = reader.Value;

				abObj.material = "";
				if (reader.GetAttribute ("material") != null) {

					reader.MoveToAttribute("material");
					abObj.material = reader.Value;
				}
					
				reader.MoveToAttribute("x");
				abObj.x = (float)Convert.ToDouble(reader.Value);

				reader.MoveToAttribute("y");
				abObj.y = (float)Convert.ToDouble(reader.Value);

				abObj.rotation = 0f;
				if (reader.GetAttribute ("rotation") != null) {
				
					reader.MoveToAttribute ("rotation");
					abObj.rotation = (float)Convert.ToDouble (reader.Value);
				}

				if (nodeName == "Block") {

					level.blocks.Add ((OBjData)abObj);
					reader.Read ();
				} 
				else if (nodeName == "Pig") {

					level.pigs.Add ((OBjData)abObj);
					reader.Read ();
				}
				else if (nodeName == "Platform") {

					abObj.width = 1;
					if (reader.GetAttribute ("width") != null) {

						reader.MoveToAttribute ("width");
						abObj.width = (int)Convert.ToInt32 (reader.Value);
					}

					abObj.height = 1;
					if (reader.GetAttribute ("height") != null) {

						reader.MoveToAttribute ("height");
						abObj.height = (int)Convert.ToInt32 (reader.Value);
					}

					level.platforms.Add (abObj);
					reader.Read ();
				}
			}
		}

		return level;
	}

	public static void SaveXmlLevel(ABLevel level, string path) {

		StringBuilder output = new StringBuilder();
		XmlWriterSettings ws = new XmlWriterSettings();
		ws.Indent = true;

		using (XmlWriter writer = XmlWriter.Create(output, ws))
		{
			writer.WriteStartElement("Level");
			writer.WriteAttributeString("width", level.width.ToString());

			writer.WriteStartElement("BirdsAmount");
			writer.WriteValue(level.birdsAmount);
			writer.WriteEndElement();

			writer.WriteStartElement("Slingshot");
			writer.WriteAttributeString("x", level.slingshot.x.ToString());
			writer.WriteAttributeString("y", level.slingshot.y.ToString());
			writer.WriteEndElement();

			writer.WriteStartElement("GameObjects");

			foreach(OBjData abObj in level.blocks)
			{
				writer.WriteStartElement("Block");
				writer.WriteAttributeString("type", abObj.type.ToString());
				writer.WriteAttributeString("material", abObj.material.ToString());
				writer.WriteAttributeString("x", abObj.x.ToString());
				writer.WriteAttributeString("y", abObj.y.ToString());
				writer.WriteAttributeString("rotation", abObj.rotation.ToString());
				writer.WriteEndElement();
			}

			foreach(OBjData abObj in level.pigs)
			{
				writer.WriteStartElement("Pig");
				writer.WriteAttributeString("type", abObj.type.ToString());
				writer.WriteAttributeString("material", abObj.material.ToString());
				writer.WriteAttributeString("x", abObj.x.ToString());
				writer.WriteAttributeString("y", abObj.y.ToString());
				writer.WriteAttributeString("rotation", abObj.rotation.ToString());
				writer.WriteEndElement();
			}

			foreach(PlatData abObj in level.platforms)
			{
				writer.WriteStartElement("Platform");
				writer.WriteAttributeString("type", abObj.type.ToString());
				writer.WriteAttributeString("material", abObj.material.ToString());
				writer.WriteAttributeString("x", abObj.x.ToString());
				writer.WriteAttributeString("y", abObj.y.ToString());
				writer.WriteAttributeString("rotation", abObj.rotation.ToString());
				writer.WriteAttributeString("width", abObj.width.ToString());
				writer.WriteAttributeString("height", abObj.height.ToString());
				writer.WriteEndElement();
			}
		}
			
		StreamWriter streamWriter = new StreamWriter(path);
		streamWriter.WriteLine(output.ToString());
		streamWriter.Close();
	}
		
	public static Dictionary<string, GameObject> LoadABResource(string path) {

		// Load block templates and cast them to game objects
		UnityEngine.Object[] objs = Resources.LoadAll(path);

		Dictionary<string, GameObject> resources = new Dictionary<string, GameObject>();

		for (int i = 0; i < objs.Length; i++) {

			GameObject abGameObject = (GameObject)objs[i];
			resources [abGameObject.name] = abGameObject;
		}

		return resources;
	}

	public void SaveLevelOnScene() {

//		Transform blocksInScene = GameWorld.Instance.BlocksInScene();
//
//		List<GameObject> objsInScene = GameWorld.Instance.BlocksInScene();

//		ABLevel level = ABLevelGenerator.GameObjectsToABLevel(objsInScene.ToArray());
//		SaveXmlLevel(level);
	}
}