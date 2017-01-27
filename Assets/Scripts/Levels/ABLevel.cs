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
using System.Collections;
using System.Collections.Generic;

public class OBjData {

	public string type;
	public string material;
	public float  rotation;
	public float  x, y;
}

public class PlatData : OBjData {

	public int width;
	public int height;
}

public struct CameraData {

	public float minWidth;
	public float maxWidth;
	public float x, y;
}

public struct SlingData {

	public float x, y;
}

public class ABLevel 
{
	public int birdsAmount;
	public int width;

	public CameraData camera;
	public SlingData slingshot;

	public List<OBjData> pigs;
	public List<OBjData> blocks;
	public List<PlatData> platforms;

	public static readonly int BIRDS_MAX_AMOUNT = 5;

	public ABLevel() {

		birdsAmount = 0;
		width = 1;

		pigs = new List<OBjData>();
		blocks = new List<OBjData>();
	}
}