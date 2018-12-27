using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFramework
{
    public class GuidedSellerValidationMessages
    {
        // guided seller pest control
        public readonly string PageTitle = "Pest Problem Solver Demo";
        public readonly string MainBreadcrumbs = "Pest Solutions Guide / Outdoor / Plant Care";
        public readonly string CartLabel = "Cart";
        public readonly string FooterText = "More saving. More doing.®";
        public readonly string MainText = "Recommended Products";
        public readonly string SubText = "Below are our top products to solve your problem, based on your previous selections.";
        public readonly string GetItFastLabel = "Get It Fast";
        public readonly string PickUpTodayLabel = "Pick Up Today";
        public readonly string DefProdDesc = "Yard and Garden Insect Killer";
        public readonly string GoalText = "What problem can we help you solve?";
        //public readonly string GoalText = "Goal";
        public readonly string[] PestSolutionsMainChoiceList = { "Kill Weeds", "Cure Lawn Patches", "Cure Plant Disease", "Control Insects", "Control Animals", "Control Rodents" };
        public readonly string SelectedMainChoice = "rgba(249, 99, 2, 1)";
        public readonly string NoRecommendedProdMsg = "Make a selection above to view recommended products.";

        //Add to Cart
        //public readonly string CD_AddedItemText = "1 Item Added to Cart";
        public readonly string CD_AddedItemText = "1 Item(s) Added to Cart";
        public readonly string ActiveBackgroundColor = "rgba(249, 99, 2, 1)";

        //Add to Cart - Inventory dialog
        public readonly string PU_Header = "Confirm Your Pickup Store";
        public readonly string PU_SubText = "Is this where you'd like to pick up your order?";
        public readonly string PU_ChangePickupStore = "Change Pickup Store";
        public readonly string PU_PickupButton = "Yes, I'll Pick Up Here";

        //Add to Cart - confirmation dialog
        public readonly string CD_CheckoutNowBtn = "Checkout Now";
        public readonly string CD_ViewCart = "View Cart";
        public readonly string CD_TaxesText = "Taxes are calculated during checkout.";
        public readonly string CD_SuggestedItemsText = "Suggested Items with Your Purchase";

        //Local store
        public readonly string LocalStoreModalHeader = "Change Your Local Store";


        // Appliances
        public readonly string[] AppliancesMainChoiceList = { "Refrigerators", "Ranges", "Dishwashers", "Microwaves" };

    }
}
