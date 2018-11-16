using SkdRefSiteAPI.DAO.Models;
using SkdRefSiteAPI.DAO.Models.Animals;
using SkdRefSiteAPI.DAO.Models.People;
using SkdRefSiteAPI.DAO.Models.Structures;
using SkdRefSiteAPI.DAO.Models.Vegetation;
using SkdRefSiteAPI.DAO.Queryables;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkdRefSiteAPI.DAO
{
    public class ReferenceDAOFactory
    {
        public static ReferenceDAO<AnimalReference, AnimalClassifications> GetAnimalsDAO()
        {
            return new ReferenceDAO<AnimalReference, AnimalClassifications>(ReferenceType.Animal, new AnimalsQueryable());
        }

        public static ReferenceDAO<FullBodyReference, FullBodyClassifications> GetFullBodiesDAO()
        {
            return new ReferenceDAO<FullBodyReference, FullBodyClassifications>(ReferenceType.FullBody, new FullBodiesQueryable());
        }

        public static ReferenceDAO<BodyPartReference, BodyPartClassifications> GetBodyPartsDAO()
        {
            return new ReferenceDAO<BodyPartReference, BodyPartClassifications>(ReferenceType.BodyPart, new BodyPartsQueryable());
        }

        public static ReferenceDAO<StructureReference, StructureClassifications> GetStructuresDAO()
        {
            return new ReferenceDAO<StructureReference, StructureClassifications>(ReferenceType.Structure, new StructuresQueryable());
        }

        public static ReferenceDAO<VegetationReference, VegetationClassifications> GetVegetationsDAO()
        {
            return new ReferenceDAO<VegetationReference, VegetationClassifications>(ReferenceType.Vegetation, new VegetationQueryable());
        }
    }
}
