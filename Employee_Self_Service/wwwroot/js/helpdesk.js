$(document).ready(function () {
    function populateSubCategories(categoryId) {
        if (categoryId == 3) {
            // Hide the standard dropdown and show the checkbox container
            $('#SubCategories').addClass('d-none');
            $('#subCategoryCheckboxContainer').removeClass('d-none');

            // Fetch subcategories and populate the checkbox menu
            $.getJSON('@Url.Action("GetSubCategories", "HelpDesk")', { categoryId: categoryId }, function (subCategories) {
                const subCategoryCheckboxMenu = $('#subCategoryCheckboxMenu');
                subCategoryCheckboxMenu.empty();

                $.each(subCategories, function (index, subCategory) {
                    const checkboxItem = `
                        <li>
                            <div class="form-check">
                                <input class="form-check-input subCategoryCheckbox" type="checkbox" value="${subCategory.value}" id="subCategory_${subCategory.value}">
                                <label class="form-check-label" for="subCategory_${subCategory.value}">
                                    ${subCategory.text}
                                </label>
                            </div>
                        </li>
                    `;
                    subCategoryCheckboxMenu.append(checkboxItem);
                });

                // Update selected subcategories when checkboxes are clicked
                $('.subCategoryCheckbox').change(function () {
                    updateSelectedSubCategories();
                });
            });
        } else {
            // Show the standard dropdown and hide the checkbox container
            $('#SubCategories').removeClass('d-none');
            $('#subCategoryCheckboxContainer').addClass('d-none');

            // Fetch subcategories and populate the standard dropdown
            $.getJSON('@Url.Action("GetSubCategories", "HelpDesk")', { categoryId: categoryId }, function (subCategories) {
                const subCategoriesSelect = $('#SubCategories');
                subCategoriesSelect.empty().append('<option value="">Select Sub Category</option>');
                $.each(subCategories, function (index, subCategory) {
                    subCategoriesSelect.append($('<option/>', {
                        value: subCategory.value,
                        text: subCategory.text
                    }));
                });

                $('#SubCategories').valid();
            });
        }
    }

    function updateSelectedSubCategories() {
        const selectedSubCategories = [];
        const selectedSubCategoryIds = [];

        $('.subCategoryCheckbox:checked').each(function () {
            selectedSubCategories.push($(this).next('label').text());
            selectedSubCategoryIds.push($(this).val());
        });

        $('#SubCategories').val(selectedSubCategoryIds.join(','));
    }

    // Handle category change
    $('#Categories').change(function () {
        const categoryId = $(this).val();
        populateSubCategories(categoryId);
    });
});
