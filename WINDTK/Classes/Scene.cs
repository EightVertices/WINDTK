using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WINDXN.Classes
{
    public class Scene
    {
        public Engine Engine;

        // Scene variables
        public string Name = "";
        public List<GameObject> SceneObjects = new List<GameObject>();

        public Scene(string name)
        {
            Name = name;
        }

        public virtual void Initialize()
        {
            GenerateScene();
        }

        public virtual void Update(ref GameTime gameTime)
        {
            for (int i = 0; i < SceneObjects.Count; i++)
            {
                SceneObjects[i].Update(ref gameTime);
            }
        }

        public virtual void Render(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < SceneObjects.Count; i++)
            {
                SceneObjects[i].Render(spriteBatch);
            }
        }

        protected virtual void GenerateScene()
        {
            WXNFile file = new WXNFile("Scenes/" + Name + ".wxn");

            Dictionary<string, dynamic> SceneData = file.Read();

            Console.WriteLine(SceneData["SceneObjects"][0]);
            Console.WriteLine(SceneData["SceneObject (Just One)"]);
            Console.WriteLine(SceneData["RandomNumberIDK"]);
            Console.WriteLine(SceneData["RandomNumberIDKasAnArrayBecauseWhyNot"][0]);

            // Gnerating scene
            /*for (int i = 0; i < SceneData["SceneObjects"].Length; i++)
            {
                AddObject(Activator.CreateInstance(Type.GetType(SceneData["SceneObjects"][i])));
            }*/
        }

        // Utility functions
        public void AddObject(GameObject _object)
        {
            SceneObjects.Add(_object);
            _object.Engine = Engine;
            _object.CurrentScene = this;
            _object.Initialize();
        }

        public void RemoveObject(GameObject _object)
        {
            for (int i = 0; i < SceneObjects.Count; i++)
            {
                if (SceneObjects[i] == _object)
                {
                    SceneObjects.RemoveAt(i);
                }
            }
        }
    }
}
