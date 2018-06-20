
//Self-Executing Anonymous Function: Part 2 (Public & Private)
(function (sqlHealthMonitor, $, undefined) {
    //Private Property
    var errorBlock = "#errorBlock";
    var okBlock = "#okBlock";
    var modalDialogBlock = "#modalDialogBlock";
    //Public Property
   
    //Public Method

    /**
     * Show Modal dialog based in Bootsrap with two options OK/Cancle,if Cancle is pushed, callBack is called
     * @param {string} title title
     * @param {string} message message
     * @param {object} callBack callBack
     */
    sqlHealthMonitor.showModalDialog = function showModalDialog(title, message, callBack) {
        var returnValue = false;
        var text = "<div class='modal' id='modalDialog'>" +
            "<div class='modal-dialog modal-dialog-centered'>" +
            "<div class='modal-content'>" +
            "<div class='modal-header'>" +
            "<h4 class='modal-title'>" + title + "</h4>" +
            //"<button type='button' class='close' data-dismiss='modal'>&times;</button>" +
            "</div>" +
            "<div class='modal-body'>" + message +
            "</div>" +
            "<div class='modal-footer'>" +
            "<button type='button' id='modalDialog_butOK'  class='btn btn-primary'>OK</button>" +
            "<button type='button' id='modalDialog_butCancle'  class='btn btn-danger'>Close</button>" +
            "</div></div></div></div>";
        /**
         * Remove dialog from HTML
         */
        function removeDialog() {
            $('#modalDialog').modal('hide');
            $('#modalDialog').modal('dispose');
            $('#modalDialog').remove();
        }
        //initialization
        $(modalDialogBlock).append(text);
        $("#modalDialog_butOK").on("click", function () { removeDialog(); callBack(); });
        $("#modalDialog_butCancle").on("click", function () { removeDialog(); });
        $("#modalDialog").modal('show');
        //events

    };

    /**
     * Method that executes Ajax Async calls and waiting for JSON content from server
     * if it's just Action without result,you should return JSON {Result: "OK",Message: <something>} to show to a user Success
     * if error occured,return JSON {Result: "ERROR",Message: <something>}
     * @param {string} url url for JsonRequest
     * @param {object} data JsonObject as request,Json.stringify is carried out
     * @returns {object} JSON {Result: "ERROR",Message: <something>}.
     */
    sqlHealthMonitor.jsonRequest = function jsonRequest(url, data = null) {
        return new Promise(function (resolve, reject) {
            var jsonResponse = null;
            const xhr = new XMLHttpRequest();
            xhr.onreadystatechange = function (e) {
                if (xhr.readyState === 4) {
                    if (xhr.status === 200) {
                        try {
                            jsonResponse = JSON.parse(xhr.response);
                        } catch (e) {
                            //json parsing error
                            reject({
                                Result: "ERROR",
                                Message: e
                            });
                        }
                        //error from server
                        if (jsonResponse.Result === "ERROR")
                            reject(jsonResponse);
                        else
                            resolve(jsonResponse);
                        //xhr error
                    } else {
                        reject({
                            Result: "ERROR",
                            Message: xhr.status
                        });
                    }
                }
            };
            xhr.ontimeout = function () {
                reject({
                    Result: "ERROR",
                    Message: "Timeout"
                });
            };
            xhr.open('post', url, true);
            //Send the proper header information along with the request
            if (data !== null) {
                xhr.setRequestHeader("Content-type", "application/json");
                xhr.send(JSON.stringify(data));
            }
            else
                xhr.send();
        });
    };
   /**
  * Method that executes Ajax BLOCK calls and waiting for JSON content from server
  * if it's just Action without result,you should return JSON {Result: "OK",Message: <something>} to show to a user Success
  * if error occured,return JSON {Result: "ERROR",Message: <something>}
  * @param {string} url JsonRequest
  * @param {object} data JsonObject as request,Json.stringify is carried out
  */
    sqlHealthMonitor.jsonRequestBlock = function jsonRequestBlock(url, data = null) {

        var jsonResponse = null;
        const xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function (e) {
            if (xhr.readyState === 4) {
                if (xhr.status === 200) {
                    try {
                        jsonResponse = JSON.parse(xhr.response);
                    } catch (e) {
                        //json parsing error
                        throw {
                            Result: "ERROR",
                            Message: e
                        };
                    }
                    //error from server
                    if (jsonResponse.Result === "ERROR")
                        throw jsonResponse;
                    else
                        return jsonResponse;
                    //xhr error
                } else {
                    throw {
                        Result: "ERROR",
                        Message: xhr.status
                    };
                }
            }
        };
        xhr.ontimeout = function () {
            throw {
                Result: "ERROR",
                Message: "Timeout"
            };
        };
        xhr.open('post', url, true);
        //Send the proper header information along with the request
        if (data !== null) {
            xhr.setRequestHeader("Content-type", "application/json");
            xhr.send(JSON.stringify(data));
        }
        else
            xhr.send();
    };
    /**
     * Error Dialog,displayed as bootstrap panel defined in errorBlock private prop
     * @param {string} message message
     */
    sqlHealthMonitor.showErrorDialog = function showErrorDialog(message) {
        var text = "<div class='alert alert-dismissible alert-danger'>" +
            "<button type='button' class='close' data-dismiss='alert'>&times;</button>" +
            "<strong>ouch!</strong> <a href='#' class='alert-link'>" + message + "</a></div>";
        $(errorBlock).append(text);
    };
  
    /**
     * OK Dialog,displayed as bootstrap panel defined in okBlock private prop
     * @param {string} message message
     */
    sqlHealthMonitor.showOkDialog = function showOkDialog(message) {
        var text = "<div class='alert alert- dismissible alert-success'>" +
            "<button type='button' class='close' data-dismiss='alert'>&times;</button>" +
            "<strong></strong> <a href='#' class='alert-link'>" + message + "</a></div>";
        $(okBlock).append(text);
    };
    /**
     * Go throught all element inside the parent element and set its height and width
     * @param {string} parentElement IdOfElement
     */
    sqlHealthMonitor.setDimensionByBiggest = function setDimensionByBiggest(parentElement) {
        var maxHeight = 0;
        var maxWidth = 0;
        $(parentElement).find("*").each(function () {
            var thisH = $(this).height();
            if (thisH > maxHeight) { maxHeight = thisH; }
            var thisW = $(this).width();
            if (thisW > maxWidth) { maxWidth = thisW; }
        });
        $(parentElement).width(maxWidth);
        $(parentElement).height(maxHeight);
    };



    /**
     * toggle full screen ,it makes elementId stretch all over the screen
     * @param {string} elementId IdOfelement
     */
    sqlHealthMonitor.toggleFullScreen = function toggleFullScreen(elementId) {
        function errorHandler() {
            alert('mozfullscreenerror');
        }
        var elem = document.getElementById(elementId);
        elem.addEventListener('mozfullscreenerror', errorHandler, false);
        if (!document.fullscreenElement &&    // alternative standard method
            !document.mozFulflScreenElement && !document.webkitFullscreenElement) {  // current working methods
            if (elem.requestFullscreen) {
                elem.requestFullscreen();
            } else if (elem.mozRequestFullScreen) {
                elem.mozRequestFullScreen();
            } else if (elem.webkitRequestFullscreen) {
                elem.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
            }
        } else {
            if (document.cancelFullScreen) {
                document.cancelFullScreen();
            } else if (document.mozCancelFullScreen) {
                document.mozCancelFullScreen();
            } else if (document.webkitCancelFullScreen) {
                document.webkitCancelFullScreen();
            }
        }
    };

   

    //Private Method
    
}(window.sqlHealthMonitor = window.sqlHealthMonitor || {}, jQuery));
