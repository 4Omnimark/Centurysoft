/*
DropMenu v1.0.3
Neptune Century Studios (NCS)
http://www.neptunecentury.com

Copyright (c) 2014, Eric Butler

Licensed under MIT (http://opensource.org/licenses/MIT)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

(function ($) {


    $.fn.dropMenu = function (options) {


        //rotate function
        function rotate(target, deg, duration, easing) {
            $(target).animate({ borderSpacing: deg },
                {
                    duration: duration,
                    easing: easing,
                    step: function (x, fx) {
                        $(this).css('transform', 'rotate(' + x + 'deg)');
                    }
                }
            );
        }

        //get the data attributes from the element
        function getDataAttributes($element) {
            var opts = {
                expandSpeed: $element.data("expand-speed"),
                expandEasing: $element.data("expand-easing"),
                retractSpeed: $element.data("retract-speed"),
                retractEasing: $element.data("retract-easing"),
                height: $element.data("height"),
                expandComplete: $element.data("expand-complete"),
                retractComplete: $element.data("retract-complete"),
                toggler: $element.data("toggler"),
                indicator: $element.data("indicator")
            };

            return $.extend({}, $.fn.dropMenu.defaultOptions, opts);;
        }

        //set up our menu
        return this.each(function () {
            var $element = $(this);

            var opts = getDataAttributes($element);
            //store our options for later use
            opts = $.extend({}, opts, options);


            $element.data("options", opts);

            //attach the options to each element
            $element.data("expanded", false);

            //use the toggler (a selector) to open/close our dropmenu
            $(opts.toggler).click(toggleMenu);

            //toggle menu
            function toggleMenu(e) {
                ev = event || e;
                //first check to see if the element is in an animated state
                if ($element.is(':animated'))
                    return;

                var options = $element.data("options");
                var expanded = $element.data("expanded");

                //decide which way the menu goes
                if (!expanded) {
                    //drop open the menu
                    $element.animate({
                        height: options.height
                    },
                    {
                        duration: options.expandSpeed,
                        easing: options.expandEasing,
                        complete: function () {

                            //set click event on body (this will close the menu
                            $("body").on("click.dropmenu", function () {
                                if (expanded) {
                                    toggleMenu();
                                }
                            });

                            //change state
                            expanded = true;
                            $element.data("expanded", expanded);

                            //fire the user defined function
                            if (options.expandComplete) {
                                //fire event
                                if (typeof options.expandComplete == 'string') {
                                    options.expandComplete = eval(options.expandComplete);
                                }

                                options.expandComplete();
                            }
                        }


                    });

                    //rotate the indicator
                    rotate(options.indicator, 180, options.expandSpeed, options.expandEasing);
                }
                else {

                    //collapse the menu. height is set to -1 because some browsers (*ahem* FireFox) don't work right
                    //when its 0.
                    $element.animate({
                        height: -1
                    },
                    {
                        duration: options.retractSpeed,
                        easing: options.retractEasing,
                        complete: function () {

                            //change state
                            expanded = false;
                            $element.data("expanded", expanded);

                            //fire the user defined function
                            if (options.retractComplete) {
                                //fire event
                                if (typeof options.retractComplete == 'string') {
                                    options.retractComplete = eval(options.retractComplete);
                                }

                                options.retractComplete();

                            }
                        }
                    });

                    $("body").off("click.dropmenu");

                    //rotate the indicator
                    rotate(options.indicator, 0.1, options.retractSpeed, options.retractEasing);

                }

                //prevent the event from bubbling up to the body, which would call this again
                ev.stopPropagation();
            }


            //prevent triggering close if we click on the menu itself
            $element.click(function (e) {
                var ev = event || e;
                //if we click on the menu, prevent the
                //event from bubbling up to body
                ev.stopPropagation();
            });

        });

    };

    //set up default options here
    $.fn.dropMenu.defaultOptions = {
        expandSpeed: 750,               //speed (in milliseconds) at which the menu expands
        expandEasing: 'easeOutBack',    //specifies the easing method of the expansion
        retractSpeed: 350,              //speed at which the menu retracts
        retractEasing: 'easeInBack',    //retract easing method
        height: 450,                    //how high (tall) the menu is when fully expanded
        expandComplete: null,           //function that executes when the menu has fully expanded
        retractComplete: null,          //function that executes when the menu has fully retracted
        toggler: null,                  //selector that specifies which element triggers the menu to expand/retract
        indicator: null                 //selector that specifies an element which spins to indicate the menu's expanded state
    };

    //find any elements with data-role=dropmenu
    $('*[data-role="dropmenu"').dropMenu();

})(jQuery);

