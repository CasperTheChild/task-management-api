import { useState , useEffect } from "react";

export default function EditTodoForm({id, todo, onEdit, onClose}) {
	const [title, setTitle] = useState(todo.title);
	const [description, setDescription] = useState(todo.description);
	const [startDate, setStartDate] = useState(todo.startDate);

	useEffect(() => {
		setTitle(todo.title);
		setDescription(todo.description);
		setStartDate(todo.startDate);
	}, [todo]);

	function handleSubmit(event) {
		event.preventDefault();
		console.log("Editing item:", title, description, startDate);

		const updatedTodo = { title, description, startDate };
		onEdit(id, updatedTodo);
		onClose();
	}

	return (
		<div>
			<h1>Edit a todo list</h1>

			<form onSubmit={handleSubmit}>
				<label htmlFor="title">Title</label>
				<input
					id="title"
					type="text"
					name="title"
					value={title}
					onChange={(e) => setTitle(e.target.value)}></input>

				<label htmlFor="description">Description</label>
				<input
					id="description"
					type="text"
					name="description"
					value={description}
					onChange={(e) => setDescription(e.target.value)}></input>

				<label htmlFor="startDate">StartDate</label>
				<input
					id="startDate"
					type="date"
					name="startDate"
					value={startDate}
					onChange={(e) => setStartDate(e.target.value)}></input>

				<button type="submit">Save</button>
				<button type="button" onClick={onClose}>Cancel</button>
			</form>
		</div>
	)
}