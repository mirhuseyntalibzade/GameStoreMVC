document.querySelectorAll('.remove-btn').forEach(button => {
    button.addEventListener('click', function (e) {
        e.preventDefault();

        const itemId = this.getAttribute('data-id');
        const itemRow = document.getElementById(`cartItem_${itemId}`);

        if (!itemRow) {
            console.error(`Row with ID cartItem_${itemId} not found.`);
            return;
        }

        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch('/Cart/RemoveItem', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ Id: parseInt(itemId) })
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            itemRow.remove();

                            Swal.fire(
                                'Deleted!',
                                'The item has been removed.',
                                'success'
                            );
                        } else {
                            Swal.fire(
                                'Error!',
                                data.message || 'Unable to remove the item.',
                                'error'
                            );
                        }
                    })
                    .catch(error => {
                        Swal.fire(
                            'Error!',
                            'An error occurred while removing the item.',
                            'error'
                        );
                        console.error('Error:', error);
                    });
            }
        });
    });
});
