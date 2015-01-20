using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameBase;
using Microsoft.Xna.Framework.Input;

namespace GameBaseDebug
{
    class StateManager : IGameStateManager
    {
        Game game;
        GameMap map1;
        bool l1;

        public StateManager()
        {
            l1 = false;
        }

        public GameMap InitializeFirstMap(Game game)
        {
            this.game = game;

            map1 = game.CreateMap(1000, 1000);
            map1.AddObject(new SpriteGameObj());
            var xx = new SpriteGameObj(150, 75);
            xx.Depth = 3;
            map1.AddObject(xx);
            //map1.AddObject(new BackgroundObject());
            map1.AddObject(new GameBase.Graphics.Background("RandomBack"));
            map1.AddObject(new BasicHUD(1100, 100));
            map1.AddObject(new ExplosionObject(300, 475));


            //MapView view1 = new MapView(new GameVector(0, 0), new GameVector(505, 0), 500, 500);
            //MapView view2 = new MapView(new GameVector(500, 0), new GameVector(505, 505), 500, 500);
            //MapView view3 = new MapView(new GameVector(500, 500), new GameVector(0, 505), 500, 500);
            //MapView view4 = new MapView(new GameVector(0, 500), new GameVector(0, 0), 500, 500);
            MapView view1 = new MapView(new GameVector(0, 0), new GameVector(0, 0), 400, 600);
            MapView view2 = new MapView(new GameVector(500, 0), new GameVector(405, 0), 400, 600);
            map1.Views.Add(view1);
            map1.Views.Add(view2);

            map1.AddObject(new Player(50, 50, true, view1));
            map1.AddObject(new Player(550, 550, false, view2));

            return map1;
        }

        public void OnUpdate()
        {
            // Allows the game to exit
            if (GameState.Keyboard.IsKeyDown(Keys.Escape))
                game.Exit();

            if (GameState.Keyboard.IsKeyDown(Keys.F))
            {
                if (!l1)
                {
                    l1 = true;
                    game.ToggleFullscreen();
                }
            }
            else
                l1 = false;

            if (GameState.Keyboard.IsKeyDown(Keys.P))
                map1.status = GameMap.STATUS.PAUSED;

            if (GameState.Keyboard.IsKeyDown(Keys.R))
                map1.status = GameMap.STATUS.RUNNING;
        }

        public void OnLoad()
        {
            //NOOP
        }

        public void OnUnload()
        {
            //NOOP
        }

        public void OnInitialize()
        {
            //NOOP
        }
    }
}
