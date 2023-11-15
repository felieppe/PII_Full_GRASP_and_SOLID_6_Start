//-------------------------------------------------------------------------
// <copyright file="Recipe.cs" company="Universidad Católica del Uruguay">
// Copyright (c) Programación II. Derechos reservados.
// </copyright>
//-------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;

namespace Full_GRASP_And_SOLID
{
    public class Recipe : IRecipeContent // Modificado por DIP
    {
        // Cambiado por OCP
        private IList<BaseStep> steps = new List<BaseStep>();

        public Product FinalProduct { get; set; }

        public bool Cooked 
        {
            get { return Cooked; }
            private set { Cooked = value; }
        }

        public Recipe() { this.Cooked = false; }

        // Agregado por Creator
        public void AddStep(Product input, double quantity, Equipment equipment, int time)
        {
            Step step = new Step(input, quantity, equipment, time);
            this.steps.Add(step);
        }

        // Agregado por OCP y Creator
        public void AddStep(string description, int time)
        {
            WaitStep step = new WaitStep(description, time);
            this.steps.Add(step);
        }

        public void RemoveStep(BaseStep step)
        {
            this.steps.Remove(step);
        }
        
        private class RecipeAdapter : TimerClient {
            Recipe Recipe;

            public RecipeAdapter (Recipe recipe) {
                this.Recipe = recipe;
            }
            public void TimeOut() {
                this.Recipe.Cooked = true;
            }
        }

        public void Cook() {
            int time = this.GetCookTime() * 1000;

            CountdownTimer timer = new CountdownTimer();
            RecipeAdapter adapter = new RecipeAdapter(this);
            timer.Register(time, adapter);

            Cooked = true;
        }

        // Agregado por SRP
        public string GetTextToPrint()
        {
            string result = $"Receta de {this.FinalProduct.Description}:\n";
            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetTextToPrint() + "\n";
            }

            // Agregado por Expert
            result = result + $"Costo de producción: {this.GetProductionCost()}";

            return result;
        }

        // Agregado por Expert
        public double GetProductionCost()
        {
            double result = 0;

            foreach (BaseStep step in this.steps)
            {
                result = result + step.GetStepCost();
            }

            return result;
        }

        public int GetCookTime() {
            int result = 0;

            foreach (BaseStep step in this.steps) {
                result += step.Time;
            }

            return result;
        }
    }
}