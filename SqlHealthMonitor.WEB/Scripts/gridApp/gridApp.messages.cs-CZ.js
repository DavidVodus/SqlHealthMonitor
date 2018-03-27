(function localization() {
    /* FlatColorPicker messages */

    if (gridApp.UserPreference) {
        gridApp.UserPreference.prototype.messages =
          {
            "preferenceSaved": "Nastavení uloženy, znovunačti stránku pro aplikaci změn",
            "preferenceNotSaved": "Nastavení neuložena"
            };
    }
    if (gridApp.KendoWrappedGrid) {
        gridApp.KendoWrappedGrid.prototype.messages =
            {
                "Days": "Dny",
                "Hours": "Hodiny",
                "Minutes": "Minuty"
            };
    }
    if (gridApp.IsResultCachedPanel) {
        gridApp.IsResultCachedPanel.prototype.messages =
            {
                "cached": "cache",
                "noCached": "databáze",
                "resultIs": "Byla použita data z",
                "notEvaluated": "dosud nevyhodnoceno"
            };
    }
    if (gridApp.GridCachedResultsWindow) {
        gridApp.GridCachedResultsWindow.prototype.messages =
            {
                "rowNumber": "Číslo řádku",
                "page": "Stránka",
                "pageSize": "Záznamů na stránku",
                "filters": "Filtry",
                "sorts": "Třídění",
                "dateOfCreation": "Datum vytvoření dotazu",
                "filtersAdded": "Filtry přidány",
                "userName": "Uživatelské jméno"
              
            };
    }
    if (gridApp.GridFiltersMultiselect) {
        gridApp.GridFiltersMultiselect.prototype.messages =
            {
               "columnBindName": "Název bindovaného sloupce",
                "value": "Hodnota",
                "operator": "logický operátor"
               

            };
    }
   

}());
