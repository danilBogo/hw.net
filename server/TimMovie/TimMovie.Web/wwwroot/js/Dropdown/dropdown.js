(function() {
    let dropdowns = $(".dropdown-filter");

    $.each(dropdowns, function() {
        const $dropdown = $(this);
        const $dropdownBtn = $dropdown.find(".dropdown-filter__button");
        const $dropdownMenu = $dropdown.find(".dropdown-filter__menu");
        const $listItems = $dropdownMenu.find(".dropdown-filter__list-item");

        const dropdownValue = $dropdown.find(".dropdown-filter__value")[0];
        const dropdownInput = $dropdown.find(".dropdown-filter__input")[0];

        let $activeItem = $dropdownMenu.find(".dropdown-filter__list-item_active");

        $dropdownBtn.on("click", function() {
            $dropdownMenu.toggle(200);
        })

        function setActiveItem($newActiveItem) {
            $activeItem.removeClass("dropdown-filter__list-item_active");
            $activeItem = $newActiveItem;
            $activeItem.addClass("dropdown-filter__list-item_active");
        }

        $.each($listItems, function() {
            $(this).on("click", function() {
                if (dropdownInput) {
                    dropdownInput.value = this.dataset.value;
                }

                if (dropdownValue) {
                    dropdownValue.innerText = this.innerText;
                }

                setActiveItem($(this));
                $dropdownBtn.trigger("click");
            })
        })

        function closeDropdown() {
            $dropdownMenu.hide(200);
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
})()