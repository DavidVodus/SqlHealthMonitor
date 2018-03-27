
window.gridApp = new function () {
    //Java Regex for ISO8601 date
    //var iso8601 = /^([\+-]?\d{4}(?!\d{2}\b))((-?)((0[1-9]|1[0-2])(\3([12]\d|0[1-9]|3[01]))?|W([0-4]\d|5[0-2])(-?[1-7])?|(00[1-9]|0[1-9]\d|[12]\d{2}|3([0-5]\d|6[1-6])))([T\s]((([01]\d|2[0-3])((:?)[0-5]\d)?|24\:?00)([\.,]\d+(?!:))?)?(\17[0-5]\d([\.,]\d+)?)?([zZ]|([\+-])([01]\d|2[0-3]):?([0-5]\d)?)?)?)?$/;
    var iso8601 = /^(-?(?:[1-9][0-9]*)?[0-9]{4})-(1[0-2]|0[1-9])-(3[01]|0[1-9]|[12][0-9])T(2[0-3]|[01][0-9]):([0-5][0-9]):([0-5][0-9])(\\.[0-9]+)?(Z)?$/;
    this.DateTimeFilter = function DateTimeFilter(control) {
        var datepicker = $(control).kendoDateTimePicker({
            format: "dd/MM/yyyy HH:mm",
            timeFormat: "HH:mm"
        });
    }
    //Control Type
    /**
     * Generic control object for kendo Components
     * @param {Object} htmlElement -obtained by $()
     * @param {Object} data - .data("kendoGrid")
     */
    var Control = function Control(htmlElement, data) {
        this.html = htmlElement;
        this.data = data;
    }

    this.UserPreference = function UserPreference(viewModel, savePreferenceUrl, popupNotification,mainGrid) {
        var savePreferenceUrl = savePreferenceUrl;
        var viewModel = viewModel;
        var popupNotification = popupNotification;
      
        this.save = function save() {
            var model = {
                viewModel: {
                    PageId: viewModel.PageId,
                    ApplicationUserId: viewModel.ApplicationUserId,
                    StartActionName: viewModel.StartActionName,
                    GridViewType: viewModel.GridViewType,
                    Columns: viewModel.Columns
                }
            };
            var columnOrder = mainGrid.getColumnOrder();
            for (var i = 0; i < columnOrder.length; i++) {
                for (var u = 0; u < model.viewModel.Columns.length; u++) {
                    if (columnOrder[i].bindName == model.viewModel.Columns[u].BindName)
                        model.viewModel.Columns[u].Order = columnOrder[i].index;

                }
            }
            $.ajax({
                url: savePreferenceUrl,
                type: "POST",
                data: {
                    viewModel: JSON.stringify(model)
                },
                success: function (response) {
                    if (response == "")
                        popupNotification.show(UserPreference.prototype.messages.preferenceSaved, "sucess");
                    else {
                        var errorModel = JSON.parse(response);
                        popupNotification.show(UserPreference.prototype.messages.preferenceNotSaved+"<br>" + errorModel.Exception.Message, "error");
                    }
                },
                error: function () {
                    popupNotification.show(UserPreference.prototype.messages.preferenceNotSaved +" Ajax error", "error");
                }
            });

        }
    }
  
        //GridPanel Type
        /**
         * 
         * @param {any} id
         * @param {any} appContext
         * @param {any} filtersMultiselect
         * @param {any} execQueryButton
         * @param {any} cancelQueryButton
         * @param {any} gridViewTypeDropDownList
         * @param {any} columnViewStateMultiselect
         * @param {any} cacheListDropDownList
         */
    this.GridPanel = function GridPanel(kendoGrid, gridPageViewModel, userPreference, filtersMultiselect, execQueryButton, cancelQueryButton,
            gridViewTypeDropDownList, columnViewStateMultiselect, cachedResultGridButton, cachedResultsWindow, savePreferenceButton,isResultCachedPanel) {
            this.gridPageViewModel = gridPageViewModel;
            this.userPreference = userPreference;
            this.grid = kendoGrid;
            this.grid.setViewType();
            this.grid.modifyPageSizeButton();
            this.isResultCachedPanel = isResultCachedPanel;
            this.filtersMultiselect = filtersMultiselect;
            this.savePreferenceButton = savePreferenceButton;
            this.savePreferenceButton.setOnClick(this, this.userPreference.save);
            this.execQueryButton = execQueryButton;
            this.execQueryButton.setOnClick(this, function (context) { context.grid.execQuery(context.filtersMultiselect); });
            this.cancelQueryButton = cancelQueryButton;
            this.cancelQueryButton.setOnClick(this, this.grid.cancelAjaxRequest);
            this.gridViewTypeDropDownList = gridViewTypeDropDownList;
            this.gridViewTypeDropDownList.setOnChange(this,this.userPreference.save ,
                this.grid.changeView);
            this.columnViewStateMultiselect = columnViewStateMultiselect;
            this.cachedResultGridButton = cachedResultGridButton;
            this.cachedResultGridButton.setOnClick(this, cachedResultsWindow.open);
            this.cachedResultsWindow = cachedResultsWindow;
        


        }
    //KendoWrappedGrid Type
    this.KendoWrappedGrid = function KendoWrappedGrid(id, gridPageViewModel, isResultCachedUrl,excelProxyUrl) {
        var control = null
        var gridPageViewModel = gridPageViewModel;
        var gridFiltersMultiselect = gridFiltersMultiselect;
        var isResultCachedPanel = isResultCachedPanel;
        var popupNotification = popupNotification;
        var ajaxRequest = undefined;
        var rowNumber = 0;
        var timeSpanTemplateEditor = function timeSpanTemplateEditor(container, options) {
            var inputContainer = $('<div id="timeSpanEditorContainer"></div>').appendTo(container);
            var inputDays = "<div class='timeSpanEditorItem'>" + KendoWrappedGrid.prototype.messages.Days + "<input class='timeSpanEditorItem' id='inputDays' name='" + options.field + ".Days'/></div>";
            var inputHours = "<div class='timeSpanEditorItem'>" + KendoWrappedGrid.prototype.messages.Hours + "<input class='timeSpanEditorItem' id='inputHours' name='" + options.field + ".Hours'/></div>";
            var inputMinutes = "<div class='timeSpanEditorItem'>" + KendoWrappedGrid.prototype.messages.Minutes + "<input class='timeSpanEditorItem' id='inputMinutes' name='" + options.field + ".Minutes'/></div>";
            $("#timeSpanEditorContainer").append(inputDays, inputHours, inputMinutes);
            $("#timeSpanEditorContainer input").kendoNumericTextBox();

        }
        var timeSpanTemplate = KendoWrappedGrid.prototype.messages.Days + " : #= CacheDuration.Days #; " + KendoWrappedGrid.prototype.messages.Hours +
            " : #= CacheDuration.Hours  #; " + KendoWrappedGrid.prototype.messages.Minutes + " : #= CacheDuration.Minutes  #"
        //in use when a template is used
        var renderRowNumber = function renderRowNumber() {
            return ++rowNumber;
        }

        /**
     * it is used in fitToscreen mode of grid
     */
        var resetRowNumber = function resetRowNumber() {
            rowNumber = 0;
        }
        /**
        * it is used in fitToscreen mode of grid
        */

        var hideNullItems = function hideNullItems() {
            //TODO:find Jquery element based on Grid element
            $(".panel-default").each(function (index) {
                var value = $(this).find(" .panel-body").text();
                if (value == 'null' || value == '')
                    $(this).hide();
            });
        }
        /**
         * Handler for showing errors occured in Grid
         * @param {any} e
         */
       var kendoGridErrorHandler = function kendoGridErrorHandler(e) {
            if (e.errors) {
                var message = "Errors:\n";
                $.each(e.errors, function (key, value) {
                    if ('errors' in value) {
                        $.each(value.errors, function () {
                            message += this + "\n";
                        });
                    }
                });
                popupNotification.show(message, "error")
           
            }
        }
       this.bindFiltersMultiselect = function bindFiltersMultiselect(control) {
           gridFiltersMultiselect = control;
           setDateTimeFilters(gridFiltersMultiselect);
       }
       this.bindIsResultCachedPanel = function bindIsResultCachedPanel(control) {
           isResultCachedPanel = control;
       }
       this.bindPopupNotification = function bindPopupNotification(control) {
           popupNotification = control;
       }
        /**
         *
         * @param gridViewType
         */
        this.changeView = function changeView(context, gridViewType, savePreferenceCallBack) {
            //save into local var
            context.gridPageViewModel.GridViewType = gridViewType;
            savePreferenceCallBack();
        }
        /**
         *
         */
        this.setViewType = function setViewType() {
            //setting gridViewType 0==excellsheet 1==fittoscreen
            if (gridPageViewModel.GridViewType == 1) {
                $(control.html[0]).addClass("fitToScreen");
            }
            if (gridPageViewModel.GridViewType == 0) {
                $(control.html[0]).removeClass("fitToScreen");
            }
        }
        /**
        * event fired when user is about click to pageSize for change amount of records in grid pre page,must be blocked
        */
        this.modifyPageSizeButton = function modifyPageSizeButton() {
            var pageSizeDropDownList = control.data.wrapper.children(".k-grid-pager").find("select").data("kendoDropDownList");
            pageSizeDropDownList.bind("select", function (e) {
                e.preventDefault();
                pageSizeDropDownList.value(e.dataItem.value);
            });

        }
           
        this.setPageSizeButtonValue = function setPageSizeButtonValue(value)
        {
            var pageSizeDropDownList = control.data.wrapper.children(".k-grid-pager").find("select").data("kendoDropDownList");
            pageSizeDropDownList.value(value);

        }
            /**
             * determine if result of Read method of Main Grid is cached or not
             * method on controller get data from Session[ResultIsCached] which is created by CacheInterceptor
             */
      
        var setDateTimeFilters = function setDateTimeFilters(filtersMultiselect) {
            //settings dateTimeFilters based on model from controller
            if (control.data.dataSource.options.schema.model === undefined)
                return;
            for (var fieldName in control.data.dataSource.options.schema.model.fields) {
                var field = control.data.dataSource.options.schema.model.fields[fieldName];
                if (field.type == 'date') {
                    var DateTimeDifference = 30;
                    $(gridPageViewModel.Columns).each(function (index) {
                        if (this.BindName == fieldName) {
                            DateTimeDifference = this.DateTimeDifference;
                        }
                    });
                    var dateTimefilter = createTodayDateTimeFilter(DateTimeDifference, fieldName);
                    if (dateTimefilter !== null)
                        filtersMultiselect.addFilter(dateTimefilter);

                }
            };
        }
        /**
         * create object contains filters from date to date based on difference in days...return null if difference == 0
         * @param {any} difference
         * @param {any} fieldName
         */
        var createTodayDateTimeFilter = function createTodayDateTimeFilter(difference, fieldName) {
            if (difference == 0)
                return null;
            var pastDate = new Date();
            pastDate.setDate(pastDate.getDate() - difference);

            var filter = {
                "logic": "and",
                "filters": [
                    {
                        "field": fieldName,
                        "operator": "gte",
                        "value": pastDate
                    },
                    {
                        "field": fieldName,
                        "operator": "lte",
                        "value": new Date()
                    }
                ]
            };
            return filter;
        }
        /**
     * Sets DateTime of datePicker filter to DateTime from Databse
     * @param {any} e
     * @param {any} appContext
     */
        var setDatePickers = function setDatePickers(e) {
            if (e.sender.dataSource.options.schema.model === undefined)
                return;
            var pageColumns = gridPageViewModel.Columns
            var field = e.sender.dataSource.options.schema.model.fields[e.field];
            if (field.type == 'date') {
                //default difference between from to To
                var DateTimeDifference = 30;
                $(pageColumns).each(function (index) {
                    if (this.BindName == e.field) {
                        DateTimeDifference = this.DateTimeDifference;
                    }
                });
                var fromDatepicker = e.container.find("[data-role=datetimepicker]:eq(0)").data("kendoDateTimePicker");
                var pastDate = new Date();
                pastDate.setDate(pastDate.getDate() - DateTimeDifference);
                fromDatepicker.value(pastDate);
                fromDatepicker.trigger("change");

                var toDatepicker = e.container.find("[data-role=datetimepicker]:eq(1)").data("kendoDateTimePicker");
                toDatepicker.value(new Date());
                toDatepicker.trigger("change");
                var beginOperator = e.container.find("[data-role=dropdownlist]:eq(2)").data("kendoDropDownList");
                beginOperator.value("lte");
                beginOperator.trigger("change");

                var endOperator = e.container.find("[data-role=dropdownlist]:eq(0)").data("kendoDropDownList");
                endOperator.value("gte");
                endOperator.trigger("change");
            }

        };
        this.setToolbar = function setToolbar(toolbar) {
            control.data.options.toolbar = toolbar;
            var options = control.data.getOptions();
            control.data.setOptions(options);
           bindEvents();
        }
        this.setTimeSpanColumnEditor = function setTimeSpanColumnEditor(fieldName) {
            var index = -1;
            $.each(control.data.columns, function (i, value) {
                if (value.field == fieldName)
                    index = i;
            });
            if (index == -1) {
                alert("index doesnt exist")
                return;
            }
            control.data.columns[index].editor = timeSpanTemplateEditor;
            control.data.columns[index].template =timeSpanTemplate ;
            //get options and set them for re-evaluation of grid Templates
            var options = control.data.getOptions();
            //save toolbar of grid cause setOptions remove it
           // var gridToolbar = $('.k-header.k-grid-toolbar.k-grid-top')[0].outerHTML;
            control.data.setOptions(options);
            //put toolbar back on grid
           //  $('.k-header.k-grid-toolbar').replaceWith(gridToolbar).html()
        }
        this.getColumnOrder = function getColumnOrder() {
            var columnsOrder = [];
            var columns = control.data.wrapper.find(".k-grid-header [data-field]");
            for (var i = 0; i < columns.length; i++) {
                var t = columns[i].attributes["data-field"].nodeValue;
                columnsOrder.push({ 'bindName': columns[i].attributes["data-field"].nodeValue, 'index': columns[i].attributes["data-index"].nodeValue });
            }
            return columnsOrder;
        }
        this.getcontrol = function getcontrol() { return control; }
        /**
         * Makes request to server for fetching data for grid
         * @param {any} context -must be context of grid
         */
        this.execQuery = function execQuery(filtersMultiselect) {
            //remove all rows from last query
            var tr = control.html.find("tbody tr").remove();
           // control.html.find("tr").remove(); 
       
            var pageSizeDropDownList = control.data.wrapper.children(".k-grid-pager").find("select").data("kendoDropDownList");
            var pageSizeValue = pageSizeDropDownList.dataItem().value;
            control.data.dataSource.query({
                filter: filtersMultiselect.getFilters(),
                pageSize: pageSizeValue
                //treshold: 5
            });

        }
        this.cancelAjaxRequest = function cancelAjaxRequest() {
            if (ajaxRequest !== undefined) {
                ajaxRequest.abort();
            }

            return false;
        }
        var bindEvents = function bindEvents() {
            //saves Ajax call from telerik Grid during a data reading.It is used for canceling actual Grid request.
            $(document).ajaxSend(function (event, jqXHR, ajaxOptions) {
                //GetRecords means it comes from telerik grid
                if (ajaxOptions.url.indexOf('GetRecords') >= 0) {
                    ajaxRequest = jqXHR;
                }
            });
            // fired after it is clicked on column filter dropDown
            control.data.unbind("filterMenuInit");
            control.data.bind("filterMenuInit", function (e) {
                setDatePickers(e);
            });
            control.data.unbind("dataBinding");
            control.data.bind("dataBinding", function () {
                resetRowNumber();
            });
            // fired when grid is about exec querying after add some filter
            control.data.unbind("filter");
            control.data.bind("filter", function (e) {
                {
                    if (gridFiltersMultiselect !== null && gridFiltersMultiselect  !== undefined) {
                        if (e.filter == null) {
                            e.preventDefault();
                            return;
                        } else {
                            gridFiltersMultiselect.addFilter(e.filter);
                            e.preventDefault();
                        }
                    }
                }

            });
            control.data.dataSource.bind("error", function (e) {
                {
                    kendoGridErrorHandler(e);
                }
            });
            // fired after datasource of grid had been started request to server
            control.data.dataSource.bind("change", function (e) {
                {
                    var data = this.data();
                }
            });
            // fired after datasource of grid had been started request to server
            control.data.dataSource.bind("requestStart", function (e) {
                {
                 
                    if (e !== undefined)
                        setLoadingAnimation(e);
                }
            });
                // fired after datasource of grid had finished the request to server
            control.data.dataSource.bind("requestEnd", function (e) {
                {
                    var response = e.response;
                    if (response === undefined)
                        return;
                    //adds functions to all Data arrays object so that 
                    //we could execute it from columns template 
                    for (var i = 0; i < response.Data.length; i++) {
                        response.Data[i].renderRowNumber = renderRowNumber;

                    }
                    if (isResultCachedPanel !== undefined && isResultCachedPanel !==null) {
                        if (response.cached !== undefined && response.cached)
                            isResultCachedPanel.set(true);
                        else
                            isResultCachedPanel.set(false);
                    }
                }
            });
           
            control.data.dataSource.bind("error",  function (e) {
                popupNotification.show("Error<br>" + e.errors, "error");
            });
        }
        var setLoadingAnimation = function setLoadingAnimation(e) {

            //clear old content of grid
          //  e.sender.data([]);
            //find proper place below filters of grid
            //TODO: get grid reference from object of Grid type
            var loadingPlaceholder = control.html.find("table").next();
           // loadingPlaceholder.hide();
            //place loading animation into placeholder
            kendo.ui.progress(loadingPlaceholder, true);
        };
        (function initializer(context) {
            control = new Control($(id), $(id).data("kendoGrid"));
            //sets excel export
            if (excelProxyUrl !== undefined && excelProxyUrl !== null) {
                control.data.options.excel = {
                    forceProxy: true,
                    fileName: "GridExport.xlsx",
                    proxyURL: excelProxyUrl,
                    filterable: true
                };
            };
            //var options = control.data.getOptions();
            ////save toolbar of grid cause setOptions remove it
            //// var gridToolbar = $('.k-header.k-grid-toolbar.k-grid-top')[0].outerHTML;
            //control.data.setOptions(options);
            bindEvents();
        }(this));

        //setViewType(appContext);
        //modifyPageSizeButton();


        }
    // IsResultCachedPanel Type
        this.IsResultCachedPanel = function IsResultCachedPanel(id) {
            var control = $(id);
            this.set = function (argument) {
                if (argument == true) {
                    control.find("span").first().text(IsResultCachedPanel.prototype.messages.cached);
                }
                else {
                    control.find("span").first().text(IsResultCachedPanel.prototype.messages.noCached);
                }
            };
            (function initializer() {
             
                control.css('border', '1px solid black');
                control.css('line-height', '25px');
                control.css('padding', '0 8px 0 8px');
                control.css('display', 'table');
                control.append(IsResultCachedPanel.prototype.messages.resultIs + " <span class='badge'>"
                    + IsResultCachedPanel.prototype.messages.notEvaluated + "</span>");

            }());
        }

    //KendoWrappedButton Type
    /**
     *
     * @param id
     */
    this.KendoWrappedButton = function KendoWrappedButton(id) {
        var control = null;
        /**
         * 
         * @param {any} context -be careful of which context you use,
         * @param {any} clickCallBack - must be in this format>example-> clickCallBack(context) ,context  is used from param. above
         */
        this.setOnClick = function setOnClick(context, clickCallBack) {
            var context = context;
           control.data.bind("click", function () { clickCallBack(context) });

        };
        (function initializer() {
            var htmlElement = $(id).kendoButton({
            });
            control = new Control(htmlElement, htmlElement.data("kendoButton"))
        }())


    }
    //KendoWrappedUpload Type
    /**
     *
     * @param id
     */
    this.KendoWrappedUpload = function KendoWrappedUpload(id) {
        var control = null;
        /**
         * 
         * @param {any} context -be careful of which context you use,
         * @param {any} uploadCallBack - must be in this format>example-> clickCallBack(context) ,context  is used from param. above
         * @param {any} removeCallBack - must be in this format>example-> clickCallBack(context) ,context  is used from param. above
         */
        this.setOnUpload = function setOnUpload(uploadCallBack) {
            var context = context;
            control.data._events.upload =uploadCallBack ;

        };
        this.setOnRemove = function setOnRemove(removeCallBack) {
            var context = context;
            control.data._events.remove  =  removeCallBack;

        };
        (function initializer() {
            var htmlElement = $(id).kendoUpload({
            });
            control = new Control(htmlElement, htmlElement.data("kendoUpload"))
        }())


    }
    //KendoWrappedUpload Type
    /**
     *
     * @param id
     */
    this.KendoUpload = function KendoUpload(id, saveUrlArg, removeUrlArg, uploadCallBack, removeCallBack,files) {
        var control = null;
        /**
         * 
         * @param {any} context -be careful of which context you use,
         * @param {any} uploadCallBack - must be in this format>example-> clickCallBack(context) ,context  is used from param. above
         * @param {any} removeCallBack - must be in this format>example-> clickCallBack(context) ,context  is used from param. above
         */
      
        (function initializer() {
            var initialFiles = JSON.parse(files);
            var htmlElement = $(id).kendoUpload({
                autoUpload:false,
                async: {
                    saveUrl: saveUrlArg,
                    removeUrl: removeUrlArg
                },
                remove: removeCallBack,
                upload: uploadCallBack,
                files: initialFiles
            });
            control = new Control(htmlElement, htmlElement.data("kendoUpload"))
        }())


    }
    //GridViewTypeDropDownList Type
    /**
     *
     * @param id
     * @param stringifyList -dataSource
     */
    this.GridViewTypeDropDownList = function GridViewTypeDropDownList(id, stringifyList, value) {
        var control = null;
        /**
         * 
         * @param {any} context -application context
         * @param {any} changeCallBack - must be in this format>example-> changeCallBack(this.value(),context) ,context  is used from param. above
         */
        this.setOnChange = function setOnChange(context, saveCallBack, changeCallBack) {
            var context = context;
            control.data.bind("change", function ()
            { changeCallBack(context, this.value(), function () { saveCallBack(context); }) });
        };
            (function initializer() {
                var htmlElement = $(id).kendoDropDownList({
                    dataTextField: "Text",
                    dataValueField: "Value",
                    value: value,
                    dataSource: JSON.parse(stringifyList)
                });
                control = new Control(htmlElement, htmlElement.data("kendoDropDownList"))
            }());
     

    }

    //GridColumnViewStateMultiselect Type
    /**
     * determine which columns of grid would be hide
     * @param id
     */
    this.GridColumnViewStateMultiselect = function GridColumnViewStateMultiselect(id,gridPageViewModel) {
        //fucking java script,if you hand columns part of grid page object on this func.,it doesnt pass itself as reference
        //so you must pass ALL gridPageView object
        var viewModel = null;
        var control = null;
        /**
      * populate control with names of column in main grid and select the one which are hiddenj
      * @param {any} gridPageViewModel
      */
        //this.setColumnsDataSource = function setColumnsDataSource(gridPageViewModel) {
        //    viewModel = gridPageViewModel;
        //    initialize(viewModel);
        //}
        /**
         * populate control with names of column in main grid and select the one which are hidden
         * @param {any} viewModel
         */
        var initialize = function initialize(e) {
            //control.data.setDataSource(viewModel.Columns);
            var dataSource = e.sender.dataSource.data();
            var selectedItems = [];
            for (var i = 0; i < dataSource.length; i++) {
                var dataItem = dataSource[i];

                if (dataItem.Hiden)
                    selectedItems.push(dataItem.GridColumnDefinitionId.toString());
            }
            e.sender.value(selectedItems);
            e.sender.trigger("change");

        };
        var hide = function (e) {
            var dataitem = e.dataItem;
            dataitem.Hiden = true;
        };
        var unHide = function (e) {
            var dataitem = e.dataItem;
            dataitem.Hiden = false;

        };

        (function initializer() {
            var htmlElement = $(id).kendoMultiSelect({
                autoClose: false,
                dataTextField: "DisplayName",
                dataValueField: "GridColumnDefinitionId",
                placeholder: "Select columns to hide",
                tagMode: "single",
                dataSource: gridPageViewModel.Columns,
                deselect: function (e) {
                    unHide(e);
                },
                select: function (e) {
                    hide(e);
                },
                dataBound: function (e) {
                    initialize(e);
                },
                change: function (e) {
                    gridPageViewModel.Columns =e.sender.dataSource.data();
                }
            });
            control = new Control(htmlElement, htmlElement.data("kendoMultiSelect"));
        }());
    }



    //GridFiltersMultiselect Type
    /**
     * shows filters which are going to be used in query to Db,user can remove them
     * @param id
     */
    this.GridFiltersMultiselect = function GridFiltersMultiselect(id) {
        var control = null;
        var filters = [];
        var filterIdPool = 0;
        var headerTemplate = '<div class="k-header itemsContainer">' +
            '<span class="k-state-default ">' + gridApp.GridFiltersMultiselect.prototype.messages.columnBindName + '</span>' +
            '<span class="k-state-default ">' + gridApp.GridFiltersMultiselect.prototype.messages.operator + '</span>' +
            '<span class="k-state-default ">' + gridApp.GridFiltersMultiselect.prototype.messages.value +'</span>' +
            '</div>';
        var itemTemplate =
            '<div class="itemsContainer">' +
            '<span class="k-state-default ">#: data.name #</span>' +
            '<span class="k-state-default ">#: gridApp.GridFiltersMultiselect.prototype.translateFilterOperator(data.operator) #</span>' +
            '<span class="k-state-default ">#: (data.value.constructor.name=="Date" ? kendo.toString(kendo.parseDate(data.value), "dd/MM/yyyy HH: mm") : data.value)  #</span>' +
        '</div>';

        /**
         * transform filters created from kendo component into flattened array
         * then add them to dataSource of kendo GridFilters
         * @param {any} arrayOfFilters
         */
        var addToDataSource = function addToDataSource(arrayOfFilters) {
            for (var i = 0; i < arrayOfFilters.length; i++) {
                if (arrayOfFilters[i].filters !== undefined) {
                    addToDataSource(arrayOfFilters[i].filters);

                }
                else {
                    arrayOfFilters[i].arrayId = filterIdPool;
                    var simplifyFieldName = arrayOfFilters[i].field.substring(arrayOfFilters[i].field.lastIndexOf(".") + 1);
                    control.data.dataSource.add({
                        arrayId: filterIdPool,
                        name: simplifyFieldName,
                        value: arrayOfFilters[i].value,
                        operator: arrayOfFilters[i].operator
                    });
                    filterIdPool++;
                }
            }
        }
        /**
         * remove filter by arrayItem.arrayId recursively
         * @param {any} arrayOfFilters
         * @param {any} filter filter we want to remove
         * @param {any} parentArray
         */
        var removeFilter = function removeFilter(arrayOfFilters, filter, parentArray, parentArrayItem) {
            for (var i = 0; i < arrayOfFilters.length; i++) {

                if (arrayOfFilters[i].filters !== undefined)
                    removeFilter(arrayOfFilters[i].filters, filter, arrayOfFilters, arrayOfFilters[i]);
                else {
                    if (arrayOfFilters[i].arrayId == filter.arrayId) {
                        arrayOfFilters.splice(i, 1);
                    }

                    if (arrayOfFilters.length == 0 && parentArray !== null) {
                        var index = parentArray.indexOf(parentArrayItem);
                        parentArray.splice(index, 1);
                    }

                }
            }

        };
        this.clearFilters = function clearFilters() {
            filters = [];

            control.data.dataSource.data([]);
            control.data.trigger("change");
        }
        this.getFilters = function getFilters() { return filters; }
        /**
         * adds filters created from kendo component to local filter array
         * @param filter
         */
        this.addFilter = function addFilter(filter) {
            filters.push(filter);
            addToDataSource(filter.filters);
            var all = $.map(control.data.dataSource.data(),
                function (dataItem) {
                    return dataItem.arrayId;
                });
            control.data.value(all);
            control.data.trigger("change");

        };

        (function initializer() {
            var htmlElement = $(id).kendoMultiSelect({
                autoBind: false,
                dataTextField: "name",
                dataValueField: "arrayId",
                placeholder: "Filters...",
                itemTemplate: itemTemplate,
                headerTemplate: headerTemplate,
          
                deselect: function (e) {
                    var dataItem = e.dataItem;
                    e.sender.dataSource.remove(dataItem);
                    e.preventDefault();
                    removeFilter(filters, dataItem, null, null);
                }
            })
            control = new Control(htmlElement, htmlElement.data("kendoMultiSelect"));

        }());
    }
    this.GridFiltersMultiselect.prototype.translateFilterOperator = function translateFilterOperator(operator) {
        var result;
        $.each(kendo.ui.FilterCell.prototype.options.operators.string, function (i, value) {
            if (i == operator)
                result = value;
        });
        if (result === undefined) {
            $.each(kendo.ui.FilterCell.prototype.options.operators.date, function (i, value) {
                if (i == operator)
                    result = value;
            });}
        return result;
    }
   


    //function ConvertByteToMac(byteArray){
    //    if(byteArray==null)
    //        return "null";
    //        var macaddress = [];
    //        for (var i=0; i < byteArray.length; i++) {
    //           var hexNumber= ("00" + byteArray[i].toString(16)).slice(-2).toUpperCase();
    //           macaddress.push(hexNumber);
    //        }
    //  return macaddress.join('.');


    //}
    //function ConvertByteToMacByCSharpMethod(byteArray){
    //    var convertedMac="";
    //    if(byteArray==null)
    //        return;
    //    $.ajax({
    //        url: '/home/test',
    //        type: "POST",
    //        async: false,
    //        data: {
    //            value: JSON.stringify(byteArray),
    //        },
    //        success: function (response) {
    //            convertedMac=  response.SomeValue;
    //        }
    //    });

    //    return  convertedMac;
    //}

    //KendoWrappedNotification Type
    this.KendoWrappedNotification = function KendoWrappedNotification(id) {
        var control = null;
       this.setOptions = function setOption(option) {
            control.data.setOptions(option);
        }
        this.show = function show(message, type) {
            if (type == "error")
                this.setOptions({ autoHideAfter: 0 });
            else
                this.setOptions({ autoHideAfter: 2000 });
            control.data.show(message, type);
        }
     
        var centerKendoNotification = function centerKendoNotification(e) {
            if (e.sender.getNotifications().length == 1) {
                var element = e.element.parent(),
                    eWidth = element.width(),
                    eHeight = element.height(),
                    wWidth = $(window).width(),
                    wHeight = $(window).height(),
                    newTop, newLeft;

                newLeft = Math.floor(wWidth / 2 - eWidth / 2);
                newTop = Math.floor(wHeight / 2 - eHeight / 2);

                e.element.parent().css({ top: newTop, left: newLeft });
            }
        };
        (function initializer() {
            var htmlElement = $(id).kendoNotification({
                stacking: "down",
                show: function (e) {
                    centerKendoNotification(e);
                }
            });
            control = new Control(htmlElement, htmlElement.data("kendoNotification"))
        }());

    }
    //KendoWrappedDialog Type
    this.KendoWrappedOkCancelDialog = function KendoWrappedOkCancelDialog(id,title,content,callBack) {
        var control = null;
        this.open = function open() {
            control.data.open();
        };
        (function initializer() {
            var htmlElement = $(id).kendoDialog({
                title: title,
                content: content,
                modal: false,
                buttonLayout: "normal",
                visible: false,
                width: "50%",
                close: function (e) {
                    // close animation has finished playing
                },
                open: function (e) {
                    // open animation will start soon
                },
                actions: [{
                    text: "OK",
                    action: function (e) {
                        callBack();
                        // e.sender is a reference to the dialog widget object
                        // OK action was clicked
                        // Returning false will prevent the closing of the dialog
                        return true;
                    },
                    primary: true
                }, {
                    text: "Cancel"
                }]
            });
        control = new Control(htmlElement, htmlElement.data("kendoDialog"))
        }());

    }
    //GridCachedResultsWindows Type

    this.GridCachedResultsWindow = function GridCachedResultsWindow(readUrl, gridFiltersMultiselect, loggedUserName, gridPageViewModel, mainGrid, popupNotification) {
        var gridId = 0;
        var windowId = 0;

        //var gridFiltersMultiselect = gridFiltersMultiselect;
        return function (readUrl, gridFiltersMultiselect, loggedUserName, gridPageViewModel, mainGrid, popupNotification) {
            var context = this;
            var control = null;
            /**
             * create grid 
             * @param {any} readUrl uri path to controller and action which handle info. about cached results
             */
            var createGrid = function createGrid(readUrl) {
                var gridName = "#grid_cachedResult" + gridId;
                //creating dataSource for grid
                var dataSource = new kendo.data.DataSource({
                    transport: {
                        serverFiltering: false,
                        cache: false,
                        read: {
                            url: readUrl,
                            dataType: "json",
                            data: {
                                queryMethodName: gridPageViewModel.QueryMethodName
                            }
                        }
                    },
                    requestEnd: function (e) {
                        //adds functions to all Data arrays object so that 
                        //we could execute it from columns template 
                        var response = e.response;
                        if (e.response === undefined)
                            return;
                        for (var i = 0; i < response.Data.length; i++) {
                            //response.Data[i].renderRowNumber = kendoWrappedGrid.renderNumber;
                            response.Data[i].generateHtmlListSortsValues = generateHtmlListSortsValues;
                            response.Data[i].generateHtmlListFiltersValues = generateHtmlListFiltersValues;
                        }
                       
                    },
                    schema: {
                        type: "json",
                        data: "Data",
                        errors: "Errors"
                    
                    }
                });
                //setting default filter according to logged user
                dataSource.filter({ field: "UserName", operator: "contains", value: loggedUserName });
                //dataSource.bind("error", dataSource_error);
                //function dataSource_error(e) {
                //    console.log(e.status); // displays "error"
                //}

             //creating Grid itself
                $(gridName).kendoGrid({
                    autoBind: false,
                    selectable: "row",
                    filterable: true,
                    sortable: true,
                    columns: [{
                        field: gridApp.GridCachedResultsWindow.prototype.messages.rowNumber,
                        sortable: false,
                        filterable: false,
                        //template: "#=function(){var pole=[];for(key in data){pole.push(key);}return pole.toString();}() #"
                        template: "#=data.renderRowNumber() #"

                    }, {
                        field: "DataSourceRequest.Page",
                        title: gridApp.GridCachedResultsWindow.prototype.messages.page
                    }, {
                        field: "DataSourceRequest.PageSize",
                        title: gridApp.GridCachedResultsWindow.prototype.messages.pageSize
                    }, {
                        field: "Filters",
                        sortable: false,
                        filterable: false,
                        template: "#=data.generateHtmlListFiltersValues(data.DataSourceRequest.Filters) #",
                        title: gridApp.GridCachedResultsWindow.prototype.messages.filters
                    }, {
                        field: "Sorts",
                        template: "#=data.generateHtmlListSortsValues(data.DataSourceRequest.Sorts) #",
                        title: gridApp.GridCachedResultsWindow.prototype.messages.sorts
                    },
                    {
                        field: "UserName",
                        template: "#=data.UserName #",
                        title: gridApp.GridCachedResultsWindow.prototype.messages.userName
                     
                    },
                    {
                        field: "Date of creation",
                        template: "#=kendo.toString(kendo.parseDate(data.Created), 'dd/MM/yyyy HH:mm')  #",
                        title: gridApp.GridCachedResultsWindow.prototype.messages.dateOfCreation
                    }
                   ],
                    //user click,then converts filter objects from Json and put them into filter control as object
                    change: function (arg) {
                        var selectedRows = this.select();
                        var dataItem = this.dataItem(selectedRows[0]);
                        var filters = dataItem.DataSourceRequest.Filters[0];

                        //a deep copy of filters obj.
                        var copiedObject = JSON.parse(JSON.stringify(filters));
                        /**
                         * convert Json date in Iso format to javascript Date
                         * @param {any} obj filters
                         */
                        var makeDateFromString = function makeDateFromString(obj) {
                            var prop = null;
                            for (prop in obj) {
                                if (iso8601.test(obj[prop]))
                                { obj[prop] = new Date(obj[prop]); }
                                else if (obj[prop].initializer === Array) {
                                    for (var i = 0, o = obj[prop]; i < obj[prop].length; i++) {
                                        makeDateFromString(obj[prop][i]);
                                    }

                                }
                            }
                        }(copiedObject);
                        //setting filters for grid
                        gridFiltersMultiselect.clearFilters();
                        gridFiltersMultiselect.addFilter(copiedObject);
                        //setting value of PageSizeButton on main Grid
                        mainGrid.setPageSizeButtonValue(dataItem.DataSourceRequest.PageSize);
                        popupNotification.show(gridApp.GridCachedResultsWindow.prototype.messages.filtersAdded, "sucess");
                       control.data.close();
                    },
                    dataSource: dataSource,
                    dataBound: function (e) {

                        for (var i = 0; i < e.sender.columns.length; i++) {
                            e.sender.autoFitColumn(i);
                        }
                    },
                    filter: function (arg) {
                        //if (arg.filter === null) {
                        //    //fix for kendo bug:data disapears from _data after start request is triggered,it causes no result is showed
                        //    arg.sender.dataSource.data([]);
                        //}

                    }

                });
                var cacheGrid = new gridApp.KendoWrappedGrid(gridName, gridPageViewModel,null,null);
                cacheGrid.bindPopupNotification(popupNotification);
               return cacheGrid;
            }
            /*Open and center the window*/
            this.open = function open() { control.data.center().open() }
          
            /**
    * Generate  html table of all FilterDescriptors applied on the Grid
    * @param {any} filters
    */
            var generateHtmlListFiltersValues = function generateHtmlListFiltersValues(filters) {
                /**
             * internal recursion traversing throught filterDescriptors
             * @param {any} filterDescriptors
             */
                var getFilterDescriptionValues = function getFilterDescriptionValues(filterDescriptors) {
                    var htmlOutput = "";
                    $.each(filterDescriptors, function (index, value) {
                        if (value.filters !== undefined) {
                            htmlOutput += getFilterDescriptionValues(value.filters);
                        }
                        else {
                            htmlOutput += "<tr>" +
                                "<td>" + value.field + "</td><td> " + (iso8601.test(value.value) ? kendo.toString(kendo.parseDate(value.value), 'dd/MM/yyyy HH:mm') : value.value) + "</td>" +
                                "</tr>";
                        }
                    });
                    return htmlOutput;
                }
                //   < tr > <th>Column</th> <th>Value</th></tr>
                var htmlOutput = "<table class='gridFiltersValuesTable'>";
                $.each(filters, function (index, value) {
                    htmlOutput += getFilterDescriptionValues(value.filters);
                });
                htmlOutput += "</table>";
                return htmlOutput;
            }
            /**
 * Generate  html table of all SortDescriptors applied on the Grid
 * @param {any} filters
 */
            var generateHtmlListSortsValues = function generateHtmlListSortsValues(sorts) {
                var htmlOutput = "<table class='gridFiltersValuesTable'>";
                $.each(sorts, function (index, value) {
                    htmlOutput += "<tr>" +
                        "<td>" + value.Member + "</td><td> " + value.SortDirection + "</td>" +
                        "</tr>";
                });
                htmlOutput += "</table>";
                return htmlOutput;
            };
            (function initializer() {
                var htmlDivElement = document.createElement('div');
                htmlDivElement.id = "gridCachedResultsWindow_" + windowId;
                //creates kendo window,element isnt destroyed after closing it, Id remains the same untill next request
                var htmlElement = $(htmlDivElement).kendoWindow({
                    title: "Cached results",
                    visible: false,
                    actions: [
                        "Pin",
                        "Minimize",
                        "Maximize",
                        "Close"
                    ],
                    close: function () {

                    },
                    activate: function (e) {
                        //reads data about cached results for inner KendoGrid,just updates dataSource with new data
                           // kendoWrappedGrid.getcontrol().data.dataSource.filter(null);
                            //kendoWrappedGrid.getcontrol().data.dataSource.query(o);
                          kendoWrappedGrid.getcontrol().data.dataSource.read();
                        

                    }
                });
                control = new Control(htmlElement, htmlElement.data("kendoWindow"))
                //append grid html element to kendo window,element isnt destroyed after closing kendo window,id of grid remains the same untill next request
                htmlElement.append("<div id='grid_cachedResult" + gridId + "'></div>");
                kendoWrappedGrid = createGrid(readUrl);
                gridId++;
                windowId++;
            }());
        }
    }()

};
