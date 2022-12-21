(function() {
    let dropdowns = $(".more-filters_one-value");

    $.each(dropdowns, function() {
        const $listItems = $(this).find(".more-filters__list-item");

        let activeItem = $(this).find(".more-filters_list-item_active");

        function setActiveItem($newActiveItem) {
            activeItem.removeClass("more-filters_list-item_active");
            $newActiveItem.addClass("more-filters_list-item_active");

            let oldMark = activeItem.find(".more-filters__list-item_mark");
            $(oldMark).removeClass("more-filters__list-item_mark_selected");
            let newMark = $newActiveItem.find(".more-filters__list-item_mark");
            $(newMark).addClass("more-filters__list-item_mark_selected");

            activeItem = $newActiveItem;
        }

        $.each($listItems, function() {
            $(this).off("click");
            $(this).on("click", function(e) {
                setActiveItem($(this));
            })
        })
    })
})()