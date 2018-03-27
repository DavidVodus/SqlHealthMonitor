(function localization() {
    /* FlatColorPicker messages */

    if (gridApp.UserPreference) {
        gridApp.UserPreference.prototype.messages =
          {
            "preferenceSaved": "Preferences has been saved,refresh page for applying change",
            "preferenceNotSaved": "Preference has NOT been saved"
            };
    }
    if (gridApp.KendoWrappedGrid) {
        gridApp.KendoWrappedGrid.prototype.messages =
            {
                "Days": "Days",
                "Hours": "Hours",
                "Minutes": "Minutes"
            };
    }
    if (gridApp.IsResultCachedPanel) {
        gridApp.IsResultCachedPanel.prototype.messages =
            {
                "cached": "cached",
                "noCached": "noCached",
                "resultIs": "result Is",
                "notEvaluated": "not Evaluated yet"
            };
    }
    if (gridApp.GridCachedResultsWindow) {
        gridApp.GridCachedResultsWindow.prototype.messages =
            {
            "rowNumber": "Row Number",
                "page": "Page",
                "pageSize": "Page size",
                "filters": "Filters",
                "sorts": "TSorts",
                "dateOfCreation": "Date Of Creation",
                "filtersAdded": "filters has been Added",
                "userName": "User Name"
              
            };
    }
    if (gridApp.GridFiltersMultiselect) {
        gridApp.GridFiltersMultiselect.prototype.messages =
            {
            "columnBindName": "column Bind Name",
                "value": "value",
                "operator": "logical operator"
               

            };
    }
   

}());
