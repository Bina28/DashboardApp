import { useState } from "react";
import api from "../api/api";

export default function ClientRow({
  client,
  isEditing,
  onEditStart,
  onEditCancel,
  onClientUpdate,
  onDelete,
}) {
  const [editFormData, setEditFormData] = useState({
    name: client.name,
    email: client.email,
  });

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setEditFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSaveClick = async () => {
    try {
      await api.put(`/clients/${client.id}`, {
        id: client.id,
        name: editFormData.name,
        email: editFormData.email,
      });

      onClientUpdate({
        ...client,
        name: editFormData.name,
        email: editFormData.email,
      });
    } catch {
      alert("Ошибка при сохранении данных клиента");
    }
  };

  if (isEditing) {
    return (
      <tr className="border-b bg-gray-50">
        <td className="p-3">
          <input
            type="text"
            name="name"
            value={editFormData.name}
            onChange={handleInputChange}
            className="border px-2 py-1 rounded"
          />
        </td>
        <td className="p-3">
          <input
            type="email"
            name="email"
            value={editFormData.email}
            onChange={handleInputChange}
            className="border px-2 py-1 rounded"
          />
        </td>
        <td className="p-3">{client.balance}</td>
        <td className="p-3">
          <button
            onClick={handleSaveClick}
            className="bg-green-600 text-white px-3 py-1 rounded mr-2 hover:bg-green-700"
          >
            Сохранить
          </button>
          <button
            onClick={onEditCancel}
            className="bg-gray-300 text-gray-800 px-3 py-1 rounded hover:bg-gray-400"
          >
            Отмена
          </button>
        </td>
      </tr>
    );
  }

  return (
    <tr className="border-b hover:bg-gray-50">
      <td className="p-3">{client.name}</td>
      <td className="p-3">{client.email}</td>
      <td className="p-3">{client.balance}</td>
      <td className="p-3 flex gap-2">
        <button
          onClick={onEditStart}
          className="bg-blue-600 text-white px-3 py-1 rounded hover:bg-blue-700"
        >
          Редактировать
        </button>
        <button
          onClick={() => onDelete(client.id)}
          className="bg-red-600 text-white px-3 py-1 rounded hover:bg-red-700"
        >
          Удалить
        </button>
      </td>
    </tr>
  );
}
