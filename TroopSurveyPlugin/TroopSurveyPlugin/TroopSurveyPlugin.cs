﻿namespace TroopSurveyPlugin
{
    using GameFreeText;
    using GameGlobal;
    using GameObjects;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using PluginInterface;
    using PluginInterface.BaseInterface;
    using System;
    using System.Drawing;
    using System.Xml;

    public class TroopSurveyPlugin : GameObject, ITroopSurvey, IBasePlugin, IPluginXML, IPluginGraphics
    {
        private string author = "clip_on";
        private const string DataPath = @"GameComponents\TroopSurvey\Data\";
        private string description = "用来显示部队概况窗口的各个属性";
        private bool enableUpdate;
        private GraphicsDevice graphicsDevice;
        private const string Path = @"GameComponents\TroopSurvey\";
        private string pluginName = "TroopSurveyPlugin";
        private bool showing;
        private TroopSurvey troopSurvey = new TroopSurvey();
        private string version = "1.0.0";
        private const string XMLFilename = "TroopSurveyData.xml";

        public void Dispose()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.Showing)
            {
                this.troopSurvey.Draw(spriteBatch);
            }
        }

        public void Initialize()
        {
        }

        public void LoadDataFromXMLDocument(string filename)
        {
            Font font;
            Microsoft.Xna.Framework.Graphics.Color color;
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            XmlNode nextSibling = document.FirstChild.NextSibling;
            XmlNode node = nextSibling.ChildNodes.Item(0);
            this.troopSurvey.BackgroundTexture = Texture2D.FromFile(this.graphicsDevice, @"GameComponents\TroopSurvey\Data\" + node.Attributes.GetNamedItem("FileName").Value);
            this.troopSurvey.BackgroundSize = new Microsoft.Xna.Framework.Point(int.Parse(node.Attributes.GetNamedItem("Width").Value), int.Parse(node.Attributes.GetNamedItem("Height").Value));
            node = nextSibling.ChildNodes.Item(1);
            this.troopSurvey.FactionTexture = Texture2D.FromFile(this.graphicsDevice, @"GameComponents\TroopSurvey\Data\" + node.Attributes.GetNamedItem("FileName").Value);
            this.troopSurvey.FactionPosition = StaticMethods.LoadRectangleFromXMLNode(node);
            node = nextSibling.ChildNodes.Item(2);
            Microsoft.Xna.Framework.Rectangle rectangle = StaticMethods.LoadRectangleFromXMLNode(node);
            StaticMethods.LoadFontAndColorFromXMLNode(node, out font, out color);
            this.troopSurvey.NameText = new FreeText(this.graphicsDevice, font, color);
            this.troopSurvey.NameText.Position = rectangle;
            this.troopSurvey.NameText.Align = (TextAlign) Enum.Parse(typeof(TextAlign), node.Attributes.GetNamedItem("Align").Value);
            node = nextSibling.ChildNodes.Item(3);
            rectangle = StaticMethods.LoadRectangleFromXMLNode(node);
            StaticMethods.LoadFontAndColorFromXMLNode(node, out font, out color);
            this.troopSurvey.KindText = new FreeText(this.graphicsDevice, font, color);
            this.troopSurvey.KindText.Position = rectangle;
            this.troopSurvey.KindText.Align = (TextAlign) Enum.Parse(typeof(TextAlign), node.Attributes.GetNamedItem("Align").Value);
            node = nextSibling.ChildNodes.Item(4);
            rectangle = StaticMethods.LoadRectangleFromXMLNode(node);
            StaticMethods.LoadFontAndColorFromXMLNode(node, out font, out color);
            this.troopSurvey.FactionText = new FreeText(this.graphicsDevice, font, color);
            this.troopSurvey.FactionText.Position = rectangle;
            this.troopSurvey.FactionText.Align = (TextAlign) Enum.Parse(typeof(TextAlign), node.Attributes.GetNamedItem("Align").Value);
            node = nextSibling.ChildNodes.Item(5);
            rectangle = StaticMethods.LoadRectangleFromXMLNode(node);
            StaticMethods.LoadFontAndColorFromXMLNode(node, out font, out color);
            this.troopSurvey.CombatTitleText = new FreeText(this.graphicsDevice, font, color);
            this.troopSurvey.CombatTitleText.Position = rectangle;
            this.troopSurvey.CombatTitleText.Align = (TextAlign) Enum.Parse(typeof(TextAlign), node.Attributes.GetNamedItem("Align").Value);
            node = nextSibling.ChildNodes.Item(6);
            rectangle = StaticMethods.LoadRectangleFromXMLNode(node);
            StaticMethods.LoadFontAndColorFromXMLNode(node, out font, out color);
            this.troopSurvey.ArmyText = new FreeText(this.graphicsDevice, font, color);
            this.troopSurvey.ArmyText.Position = rectangle;
            this.troopSurvey.ArmyText.Align = (TextAlign) Enum.Parse(typeof(TextAlign), node.Attributes.GetNamedItem("Align").Value);
            node = nextSibling.ChildNodes.Item(7);
            rectangle = StaticMethods.LoadRectangleFromXMLNode(node);
            StaticMethods.LoadFontAndColorFromXMLNode(node, out font, out color);
            this.troopSurvey.MoraleText = new FreeText(this.graphicsDevice, font, color);
            this.troopSurvey.MoraleText.Position = rectangle;
            this.troopSurvey.MoraleText.Align = (TextAlign) Enum.Parse(typeof(TextAlign), node.Attributes.GetNamedItem("Align").Value);
            node = nextSibling.ChildNodes.Item(8);
            rectangle = StaticMethods.LoadRectangleFromXMLNode(node);
            StaticMethods.LoadFontAndColorFromXMLNode(node, out font, out color);
            this.troopSurvey.CombativityText = new FreeText(this.graphicsDevice, font, color);
            this.troopSurvey.CombativityText.Position = rectangle;
            this.troopSurvey.CombativityText.Align = (TextAlign) Enum.Parse(typeof(TextAlign), node.Attributes.GetNamedItem("Align").Value);
        }

        public void SetFaction(object faction)
        {
            this.troopSurvey.ViewingFaction = faction as Faction;
            if (this.troopSurvey.ViewingFaction != null)
            {
                InformationLevel knownAreaData = this.troopSurvey.ViewingFaction.GetKnownAreaData(this.troopSurvey.TroopToSurvey.Position);
                this.enableUpdate = this.enableUpdate || (this.troopSurvey.Level != knownAreaData);
                this.troopSurvey.Level = knownAreaData;
            }
        }

        public void SetGraphicsDevice(GraphicsDevice device)
        {
            this.graphicsDevice = device;
            this.LoadDataFromXMLDocument(@"GameComponents\TroopSurvey\TroopSurveyData.xml");
        }

        public void SetTopLeftPoint(int Left, int Top)
        {
            Microsoft.Xna.Framework.Rectangle rect = new Microsoft.Xna.Framework.Rectangle(Left - this.troopSurvey.BackgroundSize.X, Top - this.troopSurvey.BackgroundSize.Y, this.troopSurvey.BackgroundSize.X, this.troopSurvey.BackgroundSize.Y);
            StaticMethods.AdjustRectangleInViewport(ref rect);
            this.troopSurvey.DisplayOffset = new Microsoft.Xna.Framework.Point(rect.X, rect.Y);
        }

        public void SetTroop(object troop)
        {
            this.enableUpdate = this.troopSurvey.TroopToSurvey != troop;
            this.troopSurvey.TroopToSurvey = troop as Troop;
        }

        public void Update(GameTime gameTime)
        {
            if (this.Showing && this.enableUpdate)
            {
                this.troopSurvey.Update();
            }
        }

        public string Author
        {
            get
            {
                return this.author;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
        }

        public object Instance
        {
            get
            {
                return this;
            }
        }

        public string PluginName
        {
            get
            {
                return this.pluginName;
            }
        }

        public bool Showing
        {
            get
            {
                return ((this.troopSurvey.TroopToSurvey != null) && this.showing);
            }
            set
            {
                this.showing = value;
            }
        }

        public string Version
        {
            get
            {
                return this.version;
            }
        }
    }
}

