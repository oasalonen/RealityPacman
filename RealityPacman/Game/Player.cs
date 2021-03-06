﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Device.Location;

namespace GhostMaps.Game
{
    public class Player : WorldObject
    {
        String _name;
        public String Name
        {
            get
            {
                if (String.IsNullOrEmpty(_name))
                {
                    return "Anonymous";
                }
                else
                {
                    return _name;
                }
            }
            set
            {
                _name = value;
            }
        }

        public int FruitsConsumed { get; set; }

        public Player() :
            this(new GeoCoordinate())
        {
        }

        public Player(GeoCoordinate position) :
            base(position)
        {
            FruitsConsumed = 0;
        }

        public void Consume(WorldObject o)
        {
            Fruit fruit = o as Fruit;
            if (fruit != null)
            {
                // Something happens?
                FruitsConsumed++;
            }
        }
    }
}
