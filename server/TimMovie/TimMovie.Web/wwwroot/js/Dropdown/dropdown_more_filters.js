(function() {
    let dropdowns = $(".more-filters");

    $.each(dropdowns, function() {
        const $dropdown = $(this);
        const $dropdownBtn = $dropdown.find(".more-filters_button");
        const $dropdownArrow = $dropdown.find(".more-filters__dropdown-arrow");
        const $listItems = $dropdown.find(".more-filters__list-item");
        const $dropdownMenu = $dropdown.find(".more-filters__menu");

        $dropdownBtn.on("click", function(e) {
            $dropdownMenu.toggle(200);
            $dropdown.toggleClass("more-filters_active");
            $dropdownArrow.toggleClass("more-filters__dropdown-arrow_rotate");
        })

        function setActiveItem($newActiveItem) {
            $newActiveItem.toggleClass("more-filters_list-item_active");
            let mark = $newActiveItem.find(".more-filters__list-item_mark");
            $(mark).toggleClass("more-filters__list-item_mark_selected");
        }

        $.each($listItems, function() {
            $(this).on("click", function(e) {
                setActiveItem($(this));
            })
        })

        function closeDropdown() {
            $dropdownMenu.hide(200);
            $dropdown.removeClass("more-filters_active");
            $dropdownArrow.removeClass("more-filters__dropdown-arrow_rotate");
        }

        $(document).on("click", function(e) {
            if (!$.contains($dropdown[0], e.target)) {
                closeDropdown();
            }
        })

        $(document).on("keydown", function(e) {
            if (e.key === "Escape") {
                closeDropdown();
            }
        })
    })

    function recalculateAmountOfColumnInGridInFilter() {
        let width = window.innerWidth;
        let newCountColumnGrid;

        if (width < 430) {
            newCountColumnGrid = 1;
        } else if (width < 680) {
            newCountColumnGrid = 2;
        } else {
            newCountColumnGrid = 3;
        }

        let filtersGrid = $(".more-filters__menu-container");
        $.each(filtersGrid, function() {
            $(this).css("grid-template-columns", `repeat(${newCountColumnGrid}, 1fr)`);
        });
    }

    function resizeWidthDropdownFilterMenu() {
        let width = window.innerWidth;
        let newWidth;

        if (width < 430) {
            newWidth = $(dropdowns[0]).css("width");
        } else
        if (width < 680) {
            newWidth = "390px";
        } else {
            newWidth = "580px";
        }

        let filtersGrid = $(".more-filters__menu-container");

        $.each(filtersGrid, function() {
            $(this).css("width", newWidth);
        });
    }

    function recalculateCountColumnInGridInContainerWithFilters() {
        let width = window.innerWidth;
        let newCountColumnGrid;

        if (width < 430) {
            newCountColumnGrid = 1;
        } else if (width < 680) {
            newCountColumnGrid = 2;
        } else {
            newCountColumnGrid = 4;
        }

        let filtersGrid = $(".additional-filters__container");
        $.each(filtersGrid, function() {
            $(this).css("grid-template-columns", `repeat(${newCountColumnGrid}, 1fr)`);
        })
    }

    function repositionFiltrMenuCountry() {
        let width = window.innerWidth;

        if (width < 430) {
            $(".more-filters__menu_country").css("left", "0");
        } else if (width < 680) {
            $(".more-filters__menu_country").css("left", "0");
            $(".more-filters__menu_country").css("right", "");
        } else {
            $(".more-filters__menu_country").css("right", "-100px");
            $(".more-filters__menu_country").css("left", "");
        }
    }

    function resizeWidthDropdownFilterMenuWithOneValue() {
        let dropdownsWithOneValue = $(".more-filters_one-value");

        $.each(dropdownsWithOneValue, function() {
            let newWidth = $(this).css("width");
            let filtersGrid = $(this).find(".more-filters__menu");
            filtersGrid.css("width", newWidth);
        });
    }

    $(document).ready(function() {
        recalculateAmountOfColumnInGridInFilter();
        resizeWidthDropdownFilterMenu();
        recalculateCountColumnInGridInContainerWithFilters();
        repositionFiltrMenuCountry();
        resizeWidthDropdownFilterMenuWithOneValue();

        $(window).on("resize", function() {
            recalculateAmountOfColumnInGridInFilter();
            resizeWidthDropdownFilterMenu();
            recalculateCountColumnInGridInContainerWithFilters();
            repositionFiltrMenuCountry();
            resizeWidthDropdownFilterMenuWithOneValue();
        })
    });
})()