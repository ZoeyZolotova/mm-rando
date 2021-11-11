﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace MMR.Randomizer.Templates
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using MMR.Randomizer.Extensions;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class HtmlSpoiler : HtmlSpoilerBase
    {
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write(@"<html>
<head>
<style>
        body.dark-mode div {
		background-color: #111;
		color: #ccc;
	}
	body.light-mode div {
		background-color: #eee;
		color: #111;
	}
    body.dark-mode {
      background-color: #111;
      color: #ccc;
    }
    body.dark-mode a {
      color: #ccc;
    }
    body.dark-mode button {
      background-color: #ddd;
      color: #111;
    }

    body.light-mode {
      background-color: #eee;
      color: #111;
    }
    body.light-mode a {
      color: #111;
    }
    body.light-mode button {
      background-color: #111;
      color: #ccc;
    }
#items{
  display: flex;
  flex: 0;
  flex-direction: column;
  max-width: 785px;
}

    th{ text-align:left }
    .region { text-align: center; font-weight: bold; }
    [data-content]:before { content: attr(data-content); }

    .dark-mode .spoiler{ background-color:#ccc }
    .dark-mode .spoiler:active { background-color: #111;  }
    .dark-mode .show-highlight .unavailable .newlocation { background-color: #500705; }
    .dark-mode .show-highlight .acquired .newlocation { background-color: #69591f; }
    .dark-mode .show-highlight .available .newlocation { background-color: #313776; }

    .light-mode .spoiler{ background-color:#111 }
    .light-mode .spoiler:active { background-color: #ccc;  }
    .light-mode .show-highlight .unavailable .newlocation { background-color: #FF9999; }
    .light-mode .show-highlight .acquired .newlocation { background-color: #99FF99; }
    .light-mode .show-highlight .available .newlocation { background-color: #9999FF; }


    #spoilerLogState { width: 560px; }

    #index {
      border: 1px solid black;
      display: flex;
      flex-direction: inline-flex;
      float: right;
      flex: 1;
      right: 10px;
      margin: 5px;
      max-height: 700px;
      width: 180px;
      justify-content: center;
      overflow-y: auto;
	  }
    .fixed {
    position: fixed;
    top: 0;
    }
    #index.light-mode {
		background-color: #eee;
		color: #111;
	}
    #index.dark-mode {
      background-color: #111;
      color: #ccc;
    }
</style>
</head>
<body class=""light-mode"">
<label><b>Version: </b></label><span>");
            this.Write(this.ToStringHelper.ToStringWithCulture(spoiler.Version));
            this.Write("</span><br/>\r\n<label><b>Settings: </b></label><code style=\"word-break: break-all;" +
                    "\">");
            this.Write(this.ToStringHelper.ToStringWithCulture(spoiler.SettingsString));
            this.Write("</code><br/>\r\n<label><b>Seed: </b></label><span>");
            this.Write(this.ToStringHelper.ToStringWithCulture(spoiler.Seed));
            this.Write("</span><br/>\r\n<br/>\r\n<button type=\"button\" onclick=\"toggleDarkLight()\" title=\"Tog" +
                    "gle dark/light mode\">Toggle Dark Theme</button>\r\n<br/>\r\n<br/>\r\n<label><b>Spoiler" +
                    " Log State: </b></label><input id=\"spoilerLogState\" type=\"text\"/><br/>\r\n");


            this.Write("<table class=\"light-mode\" id=\"index\">\n");
            this.Write("  <th><h2>Termina Index</h2></th>\n");
            foreach (var region in spoiler.ItemList.GroupBy(item => item.Region).OrderBy(g => g.Key))
            {
                this.Write("  <tr><td><a href=\"#");
                this.Write(this.ToStringHelper.ToStringWithCulture(region.Key.Name()));
                this.Write("\">");
                this.Write(this.ToStringHelper.ToStringWithCulture(region.Key.Name()));
                this.Write("</a></td></tr>\n");
            };
            this.Write("</table>\n");


            if (spoiler.DungeonEntrances.Any()) { 

            this.Write("<h2>Dungeon Entrance Replacements</h2>\r\n<table border=\"1\" class=\"item-replacement" +
                    "s\">\r\n    <tr>\r\n        <th>Entrance</th>\r\n        <th></th>\r\n        <th>New Des" +
                    "tination</th>\r\n    </tr>\r\n");
         foreach (var kvp in spoiler.DungeonEntrances) {
            var entrance = kvp.Key;
            var destination = kvp.Value;
            this.Write("    <tr data-id=\"");
            this.Write(this.ToStringHelper.ToStringWithCulture((int)destination));
            this.Write("\" data-newlocationid=\"");
            this.Write(this.ToStringHelper.ToStringWithCulture((int)entrance));
            this.Write("\" class=\"unavailable\">\r\n        <td class=\"newlocation\">");
            this.Write(this.ToStringHelper.ToStringWithCulture(entrance.Entrance()));
            this.Write("</td>\r\n        <td><input type=\"checkbox\"/></td>\r\n        <td class=\"spoiler item" +
                    "name\"><span data-content=\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(destination.Entrance()));
            this.Write("\"></span></td>\r\n    </tr>\r\n");
 } 
            this.Write("</table>\r\n");
 } 
            this.Write("<h2>Item Replacements</h2>\r\n<input type=\"checkbox\" id=\"highlight-checks\"/> Highli" +
                    "ght available checks\r\n<table border=\"1\" class=\"item-replacements\">\r\n <tr>\r\n     " +
                    "<th>Location</th>\r\n     <th></th>\r\n     <th></th>\r\n </tr>\r\n");
 foreach (var region in spoiler.ItemList.GroupBy(item => item.Region).OrderBy(g => g.Key)) {

            this.Write(" <tr class=\"region\" id=\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(region.Key.Name()));
            this.Write("\"><td colspan=\"3\">");
            this.Write(this.ToStringHelper.ToStringWithCulture(region.Key.Name()));
            this.Write("</td></tr>\r\n ");
 foreach (var item in region.OrderBy(item => item.NewLocationName)) { 
            this.Write(" <tr data-id=\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.Id));
            this.Write("\" data-newlocationid=\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.NewLocationId));
            this.Write("\" class=\"unavailable\">\r\n    <td class=\"newlocation\">");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.NewLocationName));
            this.Write("</td>\r\n    <td><input type=\"checkbox\"/></td>\r\n    <td class=\"spoiler itemname\"> <" +
                    "span data-content=\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.Name));
            this.Write("\"></span></td>\r\n </tr>\r\n ");
 } 
 } 
            this.Write("</table>\r\n<h2>Item Locations</h2>\r\n<table border=\"1\" id=\"item-locations\">\r\n <tr>\r" +
                    "\n     <th>Item</th>\r\n     <th></th>\r\n     <th>Location</th>\r\n </tr>\r\n");
 foreach (var itemCategory in spoiler.ItemList.Where(item => !item.IsJunk).GroupBy(item => item.ItemCategory).OrderBy(g => g.Key)) {

            this.Write(" <tr class=\"region\"><td colspan=\"3\">");
            this.Write(this.ToStringHelper.ToStringWithCulture(itemCategory.Key.ToString()));
            this.Write("</td></tr>\r\n ");
 foreach (var item in itemCategory.OrderBy(item => item.Id)) { 
            this.Write(" <tr data-id=\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.Id));
            this.Write("\" data-newlocationid=\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.NewLocationId));
            this.Write("\">\r\n    <td>");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.Name));
            this.Write("</td>\r\n    <td><input type=\"checkbox\"/></td>\r\n    <td class=\"spoiler newlocation\"" +
                    "> <span data-content=\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(item.NewLocationName));
            this.Write("\"></span></td>\r\n </tr>\r\n ");
 } 
 } 
            this.Write("</table>\r\n");
 if (spoiler.MessageCosts.Count > 0) { 

            this.Write("<h2>Randomized Prices</h2>\r\n<table border=\"1\">\r\n    <tr>\r\n        <th>Name</th>\r\n" +
                    "        <th>Cost</th>\r\n    </tr>\r\n");
    foreach (var (name, cost) in spoiler.MessageCosts) { 

            this.Write("    <tr>\r\n        <td>");
            this.Write(this.ToStringHelper.ToStringWithCulture(name));
            this.Write("</td>\r\n        <td class=\"spoiler\"><span data-content=\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(cost));
            this.Write("\"></span></td>\r\n    </tr>\r\n");
 } 
            this.Write("</table>\r\n");
 } 
 if (spoiler.GossipHints != null && spoiler.GossipHints.Any()) { 

            this.Write("<h2>Gossip Stone Hints</h2>\r\n<table border=\"1\">\r\n    <tr>\r\n        <th>Gossip Sto" +
                    "ne</th>\r\n        <th>Message</th>\r\n    </tr>\r\n");
    foreach (var hint in spoiler.GossipHints.OrderBy(h => h.Key.ToString())) { 

            this.Write("    <tr>\r\n        <td>");
            this.Write(this.ToStringHelper.ToStringWithCulture(hint.Key));
            this.Write("</td>\r\n        <td class=\"spoiler\"><span data-content=\"");
            this.Write(this.ToStringHelper.ToStringWithCulture(hint.Value));
            this.Write("\"></span></td>\r\n    </tr>\r\n");
 } 
            this.Write("</table>\r\n");
 } 
            this.Write("<script>\r\n    function all(list, predicate) {\r\n        for (var i = 0; i < list.l" +
                    "ength; i++) {\r\n            if (!predicate(list[i])) {\r\n                return fa" +
                    "lse;\r\n            }\r\n        }\r\n        return true;\r\n    }\r\n\r\n    function any(" +
                    "list, predicate) {\r\n        for (var i = 0; i < list.length; i++) {\r\n           " +
                    " if (predicate(list[i])) {\r\n                return true;\r\n            }\r\n       " +
                    " }\r\n        return false;\r\n    }\r\n\r\n    function includes(list, item) {\r\n       " +
                    " for (var i = 0; i < list.length; i++) {\r\n            if (list[i] === item) {\r\n " +
                    "               return true;\r\n            }\r\n        }\r\n        return false;\r\n  " +
                    "  }\r\n\r\n    function find(list, predicate) {\r\n        for (var i = 0; i < list.le" +
                    "ngth; i++) {\r\n            if (predicate(list[i])) {\r\n                return list" +
                    "[i];\r\n            }\r\n        }\r\n        return null;\r\n    }\r\n    \r\n    var segme" +
                    "ntSize = 16;\r\n    function saveItems() {\r\n        var segments = [];\r\n        fo" +
                    "r (var i = 0; i < logic.length; i++) {\r\n            var segmentIndex = parseInt(" +
                    "i / segmentSize);\r\n            segments[segmentIndex] = segments[segmentIndex] |" +
                    "| 0;\r\n            if (logic[i].Checked) {\r\n                segments[parseInt(i /" +
                    " segmentSize)] += (1 << (i%segmentSize));\r\n            }\r\n        }\r\n        var" +
                    " saveString = segments.map(function(s) {\r\n            return s.toString(16);\r\n  " +
                    "      }).join(\"-\");\r\n        var saveInput = document.querySelector(\"#spoilerLog" +
                    "State\");\r\n        saveInput.value = saveString;\r\n    }\r\n\r\n    function loadItems" +
                    "() {\r\n        var saveInput = document.querySelector(\"#spoilerLogState\");\r\n     " +
                    "   var segments = saveInput.value.split(\"-\");\r\n        if (Math.ceil((logic.leng" +
                    "th - 1) / segmentSize) !== segments.length) {\r\n            alert(\"Invalid Spoile" +
                    "r Log state\");\r\n            return;\r\n        }\r\n        segments = segments.map(" +
                    "function(segment) {\r\n            return parseInt(segment, 16);\r\n        });\r\n   " +
                    "     var locationsToCheck = [];\r\n        for (var i = 0; i < segments.length; i+" +
                    "+) {\r\n            var segment = segments[i];\r\n            for (var j = 0; j < se" +
                    "gmentSize; j++) {\r\n                var itemIndex = segmentSize * i + j;\r\n       " +
                    "         if (itemIndex < logic.length) {\r\n                    var mark = ((segme" +
                    "nt >> j) % 2 == 1);\r\n                    logic[itemIndex].Checked = mark;\r\n     " +
                    "               var itemRow = document.querySelector(\"tr[data-newlocationid=\'\" + " +
                    "itemIndex + \"\']\");\r\n                    if (itemRow) {\r\n                        " +
                    "logic[itemRow.dataset.id].Acquired = mark;\r\n                    } else {\r\n      " +
                    "                  logic[itemIndex].Acquired = mark;\r\n                    }\r\n    " +
                    "                if (!includes(locationsToCheck, itemIndex)) {\r\n                 " +
                    "       locationsToCheck.push(itemIndex);\r\n                    }\r\n               " +
                    " }\r\n            }\r\n        }\r\n        checkLocations(locationsToCheck);\r\n    }\r\n" +
                    "\r\n    document.querySelector(\"#spoilerLogState\").addEventListener(\"keypress\", fu" +
                    "nction(event) {\r\n        if (event.keyCode === 13) {\r\n            loadItems();\r\n" +
                    "        }\r\n    });\r\n\r\n    function checkLocations(locations) {\r\n        var item" +
                    "sToCheck = [];\r\n        for (var i = 0; i < locations.length; i++) {\r\n          " +
                    "  var location = logic[locations[i]];\r\n            location.IsAvailable = \r\n    " +
                    "            (location.RequiredItemIds === null || location.RequiredItemIds.lengt" +
                    "h === 0 || all(location.RequiredItemIds, function(id) { return logic[id].Acquire" +
                    "d; }))\r\n                && \r\n                (location.ConditionalItemIds === nu" +
                    "ll || location.ConditionalItemIds.length === 0 || any(location.ConditionalItemId" +
                    "s, function(conditionals) { return all(conditionals, function(id) { return logic" +
                    "[id].Acquired; }); }));\r\n            \r\n            var newLocation = find(logic," +
                    " function(io) { return io.NewLocationId === locations[i]; });\r\n            if (!" +
                    "newLocation) {\r\n                newLocation = location;\r\n            }\r\n        " +
                    "    if (!newLocation.Acquired && location.IsFakeItem && location.IsAvailable) {\r" +
                    "\n                newLocation.Acquired = true;\r\n                itemsToCheck.push" +
                    "(newLocation.ItemId);\r\n            }\r\n            if (newLocation.Acquired && lo" +
                    "cation.IsFakeItem && !location.IsAvailable) {\r\n                newLocation.Acqui" +
                    "red = false;\r\n                itemsToCheck.push(newLocation.ItemId);\r\n          " +
                    "  }\r\n        \r\n            var locationRows = document.querySelectorAll(\".item-r" +
                    "eplacements tr[data-newlocationid=\'\" + locations[i] + \"\']\");\r\n            for (c" +
                    "onst locationRow of locationRows) {\r\n                locationRow.className = \"\";" +
                    "\r\n                locationRow.classList.add(location.IsAvailable ? \"available\" :" +
                    " \"unavailable\");\r\n                var itemName = locationRow.querySelector(\".ite" +
                    "mname\");\r\n                var checkbox = locationRow.querySelector(\"input\");\r\n  " +
                    "              checkbox.checked = location.Checked;\r\n                if (location" +
                    ".Checked) {\r\n                    itemName.classList.remove(\"spoiler\");\r\n        " +
                    "        } else {\r\n                    itemName.classList.add(\"spoiler\");\r\n      " +
                    "          }\r\n            }\r\n        \r\n            var itemRows = document.queryS" +
                    "electorAll(\"#item-locations tr[data-newlocationid=\'\" + locations[i] + \"\']\");\r\n  " +
                    "          for (const itemRow of itemRows) {\r\n                var itemName = item" +
                    "Row.querySelector(\".newlocation\");\r\n                var checkbox = itemRow.query" +
                    "Selector(\"input\");\r\n                var item = logic[itemRow.dataset.id];\r\n     " +
                    "           checkbox.checked = item.Acquired;\r\n                if (item.Acquired)" +
                    " {\r\n                    itemName.classList.remove(\"spoiler\");\r\n                }" +
                    " else {\r\n                    itemName.classList.add(\"spoiler\");\r\n               " +
                    " }\r\n            }\r\n        }\r\n        if (itemsToCheck.length > 0) {\r\n          " +
                    "  checkItems(itemsToCheck);\r\n        } else {\r\n            saveItems();\r\n       " +
                    " }\r\n    }\r\n\r\n    var logic = ");
            this.Write(this.ToStringHelper.ToStringWithCulture(spoiler.LogicJson));
            this.Write(";\r\n\r\n    for (var i = 0; i < logic.length; i++) {\r\n        var item = logic[i];\r\n" +
                    "        if (item.Acquired) {\r\n            item.Checked = true;\r\n            var " +
                    "inputs = document.querySelectorAll(\"tr[data-newlocationid=\'\" + i + \"\'] input\");\r" +
                    "\n            for (const input of inputs) {\r\n                input.checked = true" +
                    ";\r\n            }\r\n        }\r\n        if (item.RequiredItemIds !== null) {\r\n     " +
                    "       for (var j = 0; j < item.RequiredItemIds.length; j++) {\r\n                " +
                    "var id = item.RequiredItemIds[j];\r\n                if (!logic[id].LocksLocations" +
                    ") {\r\n                    logic[id].LocksLocations = [];\r\n                }\r\n    " +
                    "            if (!includes(logic[id].LocksLocations, i)) {\r\n                    l" +
                    "ogic[id].LocksLocations.push(i);\r\n                }\r\n            }\r\n        }\r\n " +
                    "       if (item.ConditionalItemIds !== null) {\r\n            for (var k = 0; k < " +
                    "item.ConditionalItemIds.length; k++) {\r\n                for (var j = 0; j < item" +
                    ".ConditionalItemIds[k].length; j++) {\r\n                    var id = item.Conditi" +
                    "onalItemIds[k][j];\r\n                    if (!logic[id].LocksLocations) {\r\n      " +
                    "                  logic[id].LocksLocations = [];\r\n                    }\r\n       " +
                    "             if (!includes(logic[id].LocksLocations, i)) {\r\n                    " +
                    "    logic[id].LocksLocations.push(i);\r\n                    }\r\n                }\r" +
                    "\n            }\r\n        }\r\n    }\r\n\r\n    function checkItems(itemIds) {\r\n        " +
                    "var locationsToCheck = [];\r\n        for (var i = 0; i < itemIds.length; i++) {\r\n" +
                    "            var itemId = itemIds[i];\r\n            if (logic[itemId].LocksLocatio" +
                    "ns) {\r\n                for (var j = 0; j < logic[itemId].LocksLocations.length; " +
                    "j++) {\r\n                    var locationId = logic[itemId].LocksLocations[j];\r\n " +
                    "                   if (!includes(locationsToCheck, locationId)) {\r\n             " +
                    "           locationsToCheck.push(locationId);\r\n                    }\r\n          " +
                    "      }\r\n            }\r\n        }\r\n        checkLocations(locationsToCheck);\r\n  " +
                    "  }\r\n\r\n    var startingLocations = [0, 96, 278, 279, 280, 281];\r\n    for (var id" +
                    " of startingLocations) {\r\n        logic[id].Checked = true;\r\n        var row = d" +
                    "ocument.querySelector(\"tr[data-newlocationid=\'\" + id + \"\']\");\r\n        var itemI" +
                    "d = id;\r\n        if (row) {\r\n            itemId = row.dataset.id;\r\n            d" +
                    "ocument.querySelector(\"tr[data-newlocationid=\'\" + id + \"\'] input\").checked = tru" +
                    "e;\r\n        }\r\n        logic[itemId].Acquired = true;\r\n    }\r\n\r\n    var allLocat" +
                    "ionIds = [];\r\n    for (var i = 0; i < logic.length; i++) {\r\n        allLocationI" +
                    "ds.push(i);\r\n    }\r\n    checkLocations(allLocationIds);\r\n\r\n    var rows = docume" +
                    "nt.querySelectorAll(\"tr\");\r\n    for (var i = 1; i < rows.length; i++) {\r\n       " +
                    " var row = rows[i];\r\n        var checkbox = row.querySelector(\"input\");\r\n       " +
                    " if (checkbox) {\r\n            checkbox.addEventListener(\"click\", function(e) {\r\n" +
                    "                var row = e.target.closest(\"tr\");\r\n                var rowId = p" +
                    "arseInt(row.dataset.id);\r\n                var newLocationId = parseInt(row.datas" +
                    "et.newlocationid);\r\n                logic[newLocationId].Checked = e.target.chec" +
                    "ked;\r\n                logic[rowId].Acquired = e.target.checked;\r\n               " +
                    " checkLocations([newLocationId]);\r\n                checkItems([rowId]);\r\n       " +
                    "     });\r\n        }\r\n    }\r\n\r\n    document.querySelector(\"#highlight-checks\").ad" +
                    "dEventListener(\"click\", function(e) {\r\n        var tables = document.querySelect" +
                    "orAll(\"table.item-replacements\");\r\n        for (var i = 0; i < tables.length; i+" +
                    "+) {\r\n            if (e.target.checked) {\r\n                tables[i].classList.a" +
                    "dd(\"show-highlight\");\r\n            } else {\r\n                tables[i].classList" +
                    ".remove(\"show-highlight\");\r\n            }\r\n        }\r\n    });\r\n\r\n    function to" +
                    "ggleDarkLight() {\r\n        var body = document.getElementsByTagName(\'body\')[0];\r" +
                    "\n        var currentClassBody = body.className;\r\n        var ind = document.getElementById('index');\r\n        body.className = curren" +
                    "tClassBody === \"dark-mode\" ? \"light-mode\" : \"dark-mode\";\r\n index.className = currentClassBody === \"dark-mode\" ? \"light-mode\" : \"dark-mode\";    }\r\n");
            this.Write(@"
window.onscroll = function() { indFloat()};

            var index = document.getElementById('index');

            function indFloat()
            {
                if (window.pageYOffset > document.getElementById('index').offsetTop)
                {
                    index.classList.add('fixed');
                }
                else
                {
                    index.classList.remove('fixed');
                }
            }
" + "\r\n</script> \r\n </body>\r\n</html>\r\n");
            return this.GenerationEnvironment.ToString();
        }
    }
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public class HtmlSpoilerBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
