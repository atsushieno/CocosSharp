#if USE_PHYSICS
/****************************************************************************
 Copyright (c) 2013 Chukong Technologies Inc. ported by Jose Medrano (@netonjm)
 
 http://www.cocos2d-x.org
 
 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:
 
 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.
 
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 THE SOFTWARE.
 ****************************************************************************/

using ChipmunkSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CocosSharp
{
	internal class CCPhysicsShapeInfo
	{

		protected static Dictionary<cpShape, CCPhysicsShapeInfo> _map;
		protected List<cpShape> _shapes;

		protected CCPhysicsShape _shape;
		protected cpBody _body;
		protected int _group;
		protected static cpBody _sharedBody;

		public static Dictionary<cpShape, CCPhysicsShapeInfo> Map {
			get {


				if (_map==null)
					_map = new Dictionary<cpShape, CCPhysicsShapeInfo>();

				return _map; 
		
			} 
		}

		public static cpBody SharedBody() {

			if (_sharedBody == null)
				_sharedBody = cpBody.NewStatic(); 

			return _sharedBody; 

		}


		public CCPhysicsShapeInfo(CCPhysicsShape shape)
		{
		
			_shapes = new List<cpShape>();

			// TODO: Complete member initialization
			_shape = shape;

		
			_body = _sharedBody;

			_group = cp.NO_GROUP; //CP_NO_GROUP ?�
		}

		public CCPhysicsShape Shape { get { return _shape; } }

		public List<cpShape> GetShapes()
		{
			return _shapes;
		}

		public cpBody getBody() { return _body; }
		public int getGroup() { return _group; }



		~CCPhysicsShapeInfo()
		{
		
			_shapes.Clear();

		}

		public void SetGroup(int group)
		{
			this._group = group;

			foreach (cpShape shape in _shapes)
			{
				shape.SetFilter(new cpShapeFilter(group, cp.ALL_CATEGORIES, cp.ALL_CATEGORIES));
			}
		}

		public void SetBody(cpBody body)
		{
			if (this._body != body)
			{
				this._body = body;
				foreach (cpShape shape in _shapes)
				{
					shape.SetBody(body == null ? _sharedBody : body);
				}
			}
		}

		public void Add(cpShape shape)
		{
			if (shape == null) return;
			shape.SetFilter(new cpShapeFilter(_group, cp.ALL_CATEGORIES, cp.ALL_CATEGORIES));

			_shapes.Add(shape);
			Map.Add(shape, this);

		}

		public void Remove(cpShape shape)
		{
			if (shape == null) return;

			var it = _shapes.Find((s) => s == shape);
			if (it != null)
			{
				_shapes.Remove(it);

				CCPhysicsShapeInfo tmp;
				if (_map.TryGetValue(shape, out tmp))
					_map.Remove(shape);
			}
		
		}

		public void RemoveAll()
		{
			foreach (cpShape shape in _shapes)
			{
				CCPhysicsShapeInfo tmp;
				if (_map.TryGetValue(shape, out tmp))
					_map.Remove(shape);
			}

			_shapes.Clear();
		}

	}
}
#endif