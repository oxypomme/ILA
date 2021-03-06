﻿using ILANET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ilaGUI.Editor
{
    /// <summary>
    /// Logique d'interaction pour If.xaml
    /// </summary>
    public partial class If : UserControl, Linked, InstructionBlock, IDropableInstruction
    {
        public If()
        {
            InitializeComponent();
            App.DarkmodeUrMenus(hitbox.ContextMenu.Items);
            icon.Source = App.MakeDarkTheme(icon.Source as BitmapSource);
            EndInstruction = new DummyInstruction();
            ElseEndInstruction = new DummyInstruction();
            elifList = new List<Elif>();
            ifInstructions.Tag = this;
            elseInstructions.Tag = this;
            hitbox.ContextMenu.Tag = this;
        }

        public bool DropVisual
        {
            set
            {
                if (value)
                    insertHere.Visibility = Visibility.Visible;
                else
                    insertHere.Visibility = Visibility.Collapsed;
            }
        }

        public List<Elif> elifList { get; set; }

        public DummyInstruction ElseEndInstruction { get; set; }

        public DummyInstruction EndInstruction { get; set; }

        public IDropableInstruction[] Instructions
        {
            get
            {
                var list = new List<IDropableInstruction>();
                list.AddRange(ifInstructions.Children.Cast<IDropableInstruction>());
                foreach (var item in elifList)
                    list.AddRange(item.instructions.Children.Cast<IDropableInstruction>());
                list.AddRange(elseInstructions.Children.Cast<IDropableInstruction>());
                return list.ToArray();
            }
        }

        public ILANET.If InternalInstruction { get; set; }

        IBaseObject Linked.Link => InternalInstruction;

        public bool MovingVisual
        {
            set
            {
                if (value)
                    Background = new SolidColorBrush(Color.FromArgb(75, 50, 150, 250));
                else
                    Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
            }
        }

        public void UpdateInternalInstructions()
        {
            InternalInstruction.IfInstructions.Clear();
            InternalInstruction.ElseInstructions.Clear();
            foreach (var item in InternalInstruction.Elif)
                item.Item2.Clear();
            foreach (var item in ifInstructions.Children)
                if (!(item is DummyInstruction))
                    InternalInstruction.IfInstructions.Add((item as Linked).Link as Instruction);
            foreach (var item in elseInstructions.Children)
                if (!(item is DummyInstruction))
                    InternalInstruction.ElseInstructions.Add((item as Linked).Link as Instruction);
            for (int i = 0; i < elifList.Count; i++)
            {
                var elif = elifList[i];
                foreach (var item in elif.instructions.Children)
                    if (!(item is DummyInstruction))
                        InternalInstruction.Elif[i].Item2.Add((item as Linked).Link as Instruction);
            }
        }

        public void UpdateVisuals()
        {
            comment.Text = string.IsNullOrEmpty(InternalInstruction.Comment) ? "" : "//" + InternalInstruction.Comment;
            endComment.Text = string.IsNullOrEmpty(InternalInstruction.EndComment) ? "" : "//" + InternalInstruction.EndComment;
            conditionHolder.Content = App.GetValueControl(InternalInstruction.IfCondition, App.SymbolColorBrush.Color);
            for (int i = 0; i < InternalInstruction.Elif.Count; i++)
            {
                elifList[i].comment.Text = string.IsNullOrEmpty(InternalInstruction.ElifComments[i]) ? "" : "//" + InternalInstruction.ElifComments[i];
                elifList[i].condition.Content = App.GetValueControl(InternalInstruction.Elif[i].Item1, App.SymbolColorBrush.Color);
            }
        }

        private void hitbox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && (string)e.Data.GetData(DataFormats.StringFormat) == "" && App.Dragged != this)
            {
                DropVisual = true;
                (App.Dragged as IDropableInstruction).MovingVisual = true;
            }
        }

        private void hitbox_DragLeave(object sender, DragEventArgs e)
        {
            DropVisual = false;
            (App.Dragged as IDropableInstruction).MovingVisual = false;
        }

        private void hitbox_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && (string)e.Data.GetData(DataFormats.StringFormat) == "" && App.Dragged != this)
            {
                DropVisual = true;
                (App.Dragged as IDropableInstruction).MovingVisual = true;
                e.Effects = DragDropEffects.Move;
            }
        }

        private void hitbox_Drop(object sender, DragEventArgs e)
        {
            DropVisual = false;
            (App.Dragged as IDropableInstruction).MovingVisual = false;
            if (App.Dragged is InstructionBlock block && App.recursiveSearch(block.Instructions, this))
                return;
            if (e.Data.GetDataPresent(DataFormats.StringFormat) && (string)e.Data.GetData(DataFormats.StringFormat) == "" && App.Dragged != this)
                (this as IDropableInstruction).DropRecieved(App.Dragged as IDropableInstruction);
        }

        private void hitbox_MouseEnter(object sender, MouseEventArgs e)
        {
            hitbox.Background = new SolidColorBrush(Color.FromArgb(64, 255, 255, 255));
        }

        private void hitbox_MouseLeave(object sender, MouseEventArgs e)
        {
            hitbox.Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
        }

        private void hitbox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                App.Dragged = this;
                DragDrop.DoDragDrop(this, "", DragDropEffects.Move);
            }
        }

        public struct Elif
        {
            public TextBlock comment;
            public ContentControl condition;
            public DummyInstruction EndInstruction;
            public StackPanel instructions;
        }
    }
}